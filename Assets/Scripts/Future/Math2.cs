using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EMSP.Communication;
using System;

namespace EMSP.Future
{
    public class MathWire
    {
        public int nodes_count;
        public Vector3[] nodes;

        public Vector3[] vector_segments;
        public float[] length_segments;

        public MathWire(Wire wire)
        {
            Wire = wire;

            nodes_count = wire.LocalPoints.Count - 1;

            nodes = new Vector3[wire.LocalPoints.Count];
            for (int i = 0; i < wire.LocalPoints.Count; i++)
            {
                nodes[i] = wire.LocalPoints[i];
            }

            vector_segments = new Vector3[wire.LocalPoints.Count - 1];
            length_segments = new float[wire.LocalPoints.Count - 1];

            for (int i = 0; i < wire.LocalPoints.Count - 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    vector_segments[i][j] = wire.LocalPoints[i + 1][j] - wire.LocalPoints[i][j];
                }

                length_segments[i] = Vector3.Distance(wire.LocalPoints[i], wire.LocalPoints[i + 1]);
            }
        }

        public Wire Wire { get; private set; }
    }

    public class Math2
    {
        public struct ResultInfo
        {
            public Wire WireA { get; set; }

            public Wire WireB { get; set; }

            public float Value { get; set; }

            public void Add(float value)
            {
                Value += value;
            }
        }

        private int Factorial(int number)
        {
            if (number <= 1) return 1;

            return number * Factorial(number - 1);
        }

        private float Hypot(float a, float b)
        {
            return Mathf.Sqrt(a * a + b * b);
        }

        public ResultInfo[] Calculate(Wiring wiring)
        {
            // Проверка на количество проводов.
            if (wiring.Count < 2) throw new Exception("Проводов не может быть меньше 2х");

            float NU = 4f * Mathf.PI * Mathf.Pow(10, -7);

            int ind_m = 0;
            int count_m = Factorial(wiring.Count) / (Factorial(2) * Factorial(wiring.Count - 2));

            // Взаимная индуктивность проводов.
            ResultInfo[] M = new ResultInfo[count_m];

            // Цикл по проводам в проводке (для сравнения со следующим).
            for (int index_compare = 0; index_compare < wiring.Count - 1; index_compare++)
            {
                MathWire wire_compare = new MathWire(wiring[index_compare]);

                // Цикл по следующим проводам из списка (проводки).
                for (int index_current = index_compare + 1; index_current < wiring.Count; index_current++)
                {
                    MathWire wire_current = new MathWire(wiring[index_current]);

                    // Цикл по точкам провода (из 2го цикла).
                    for (int n_current = 0; n_current < wire_current.nodes_count; n_current++)
                    {
                        // Цикл по точкам провода (из 1го цикла).
                        for (int n_compare = 0; n_compare < wire_compare.nodes_count; n_compare++)
                        {
                            Vector3 node_compare = wire_compare.nodes[n_compare];
                            Vector3 node_current = wire_current.nodes[n_current];
                            float length_segment_compare = wire_compare.length_segments[n_compare];
                            float length_segment_current = wire_current.length_segments[n_current];

                            // Вычисление длин отрезков между проводами.
                            Dictionary<string, float> pp_segments = new Dictionary<string, float>
                            {
                                {"A1A2", (float)Math.Round(Vector3.Distance(wire_compare.nodes[n_compare], wire_current.nodes[n_current]), 3)},
                                {"B1A2", (float)Math.Round(Vector3.Distance(wire_compare.nodes[n_compare + 1], wire_current.nodes[n_current]), 3)},
                                {"A1B2", (float)Math.Round(Vector3.Distance(wire_compare.nodes[n_compare], wire_current.nodes[n_current + 1]), 3)},
                                {"B1B2", (float)Math.Round(Vector3.Distance(wire_compare.nodes[n_compare + 1], wire_current.nodes[n_current + 1]), 3)},
                            };

                            // Вычисление косинуса угла между отрезками.
                            float cos_segment = 0f;
                            for (int j = 0; j < 3; j++)
                                cos_segment += wire_compare.vector_segments[n_compare][j] * wire_current.vector_segments[n_current][j];

                            cos_segment /= length_segment_compare * length_segment_current;
                            cos_segment = (float)Math.Round(cos_segment, 3);

                            float p1x = wire_compare.vector_segments[n_compare].x;
                            float p1y = wire_compare.vector_segments[n_compare].y;
                            float p1z = wire_compare.vector_segments[n_compare].z;
                            float p2x = wire_current.vector_segments[n_compare].x;
                            float p2y = wire_current.vector_segments[n_compare].y;
                            float p2z = wire_current.vector_segments[n_compare].z;

                            // Если два отрезка параллельны либо соосны
                            if (cos_segment == 1 || cos_segment == -1)
                            {
                                float half_perimeter = (pp_segments["A1A2"] + pp_segments["B1A2"] + length_segment_compare) / 2f;
                                float max_sides_triangle = Mathf.Max(pp_segments["A1A2"], pp_segments["B1A2"], length_segment_compare);

                                // соблюдение критерия соосности.
                                if (half_perimeter == max_sides_triangle)
                                {
                                    // Отрезки расположены относительно друг друга соосно.
                                    // Расстояние перекрытия между проекциями отрезков.
                                    string key_min = null;
                                    float d = float.MaxValue;
                                    foreach (var segment in pp_segments)
                                        if (segment.Value < d)
                                        {
                                            key_min = segment.Key;
                                            d = segment.Value;
                                        }

                                    M[ind_m].Add(NU / 4f * Mathf.PI * ((length_segment_compare + length_segment_current + d)
                                        * Mathf.Log(length_segment_compare + length_segment_current + d) + d * Mathf.Log(d)
                                        - (length_segment_compare + d) * Mathf.Log(length_segment_compare + d)
                                        - (length_segment_current + d) * Mathf.Log(length_segment_current + d)
                                        * cos_segment));

                                    M[ind_m].WireA = wire_compare.Wire;
                                    M[ind_m].WireB = wire_current.Wire;
                                }
                                else
                                {
                                    // Отрезки расположены относительно друг друга параллельно
                                    float s = Mathf.Pow((half_perimeter
                                        * (half_perimeter - length_segment_compare)
                                        * (half_perimeter - pp_segments["B1A2"])
                                        * (half_perimeter - pp_segments["A1A2"])), 0.5f);

                                    // Расстояние между проводами.
                                    float h = (float)Math.Round(2f * s / length_segment_compare, 3);

                                    float d = 0;

                                    if (cos_segment == 1)
                                    {
                                        float h_b1a2 = (float)Math.Round(Mathf.Pow(Mathf.Pow(pp_segments["B1A2"], 2) - Mathf.Pow(h, 2), 0.5f), 3);
                                        float h_b1b2 = (float)Math.Round(Mathf.Pow(Mathf.Pow(pp_segments["B1B2"], 2) - Mathf.Pow(h, 2), 0.5f), 3);

                                        d = length_segment_current + h_b1a2 == h_b1b2 ? h_b1a2 : -h_b1a2;
                                    }
                                    else
                                    {
                                        float h_a1a2 = (float)Math.Round(Mathf.Pow(Mathf.Pow(pp_segments["A1A2"], 2) - Mathf.Pow(h, 2), 0.5f), 2);
                                        float h_a1b2 = (float)Math.Round(Mathf.Pow(Mathf.Pow(pp_segments["A1B2"], 2) - Mathf.Pow(h, 2), 0.5f), 2);

                                        d = length_segment_current + h_a1a2 == h_a1b2 ? h_a1a2 : -h_a1a2;
                                    }

                                    // Альфа, бета, гамма, дельта - совокупность отрезков для упрощения.
                                    float af = length_segment_compare + length_segment_current + d;
                                    float bt = length_segment_compare + d;
                                    float gm = length_segment_current + d;
                                    float dt = d;
                                    
                                    M[ind_m].Add(NU / (4 * Mathf.PI)
                                         * (af * Mathf.Log(af + Hypot(af, h))
                                         - bt * Mathf.Log(bt + Hypot(bt, h))
                                         - gm * Mathf.Log(gm + Hypot(gm, h))
                                         + dt * Mathf.Log(dt + Hypot(dt, h))
                                         - Hypot(af, h) + Hypot(bt, h)
                                         + Hypot(gm, h) - Hypot(dt, h)) * cos_segment);

                                    M[ind_m].WireA = wire_compare.Wire;
                                    M[ind_m].WireB = wire_current.Wire;
                                }
                            }
                            else
                            {
                                // Общий случай.
                                // Вычисление вектора нормали.
                                float nx = p1y * p2z - p1z * p2y;
                                float ny = -(p1x * p2z - p1z * p2x);
                                float nz = p1x * p2y - p1y * p2x;

                                // Параметры плоскостей для каждого отрезка.
                                float apl_compare, apl_current;
                                apl_compare = apl_current = nx;

                                float bpl_compare, bpl_current;
                                bpl_compare = bpl_current = ny;

                                float cpl_compare, cpl_current;
                                cpl_compare = cpl_current = nz;

                                float dpl_compare = -nx * node_compare.x - ny * node_compare.y - nz * node_compare.z;
                                float dpl_current = -nx * node_current.x - ny * node_current.y - nz * node_current.z;

                                // параметр прямой заданной параметрически которая является
                                float t1 = (-dpl_current - apl_current * node_compare.x - bpl_current * node_compare.y
                                    - cpl_current * node_compare.z) / (Mathf.Pow(apl_current, 2f) + Mathf.Pow(bpl_current, 2f)
                                    + Mathf.Pow(cpl_current, 2f));

                                // Координаты точки a1нов
                                float a1bx = node_compare.x + t1 * apl_current;
                                float a1by = node_compare.y + t1 * bpl_current;
                                float a1bz = node_compare.z + t1 * cpl_current;

                                // Находим координаты точки о2
                                float o2x = 0;
                                float o2y = 0;
                                float o2z = 0;

                                if (p1y * p2x - p2y * p1x == 0)
                                {
                                    o2x = (a1bx * p1z * p2x - node_current.x * p2z * p1x - a1bz * p1x * p2x
                                        + node_current.z * p1x * p2x) / (p1z * p2x - p2z * p1x);

                                    o2y = (a1by * p1z * p2y - node_current.y * p2z * p1y - a1bz * p1y * p2y
                                        + node_current.z * p1y * p2y) / (p1z * p2y - p2z * p1y);

                                    o2z = (a1bz * p1y * p2z - node_current.z * p2y * p1z - a1by * p1z * p2z
                                        + node_current.y * p1z * p2z) / (p1y * p2z - p2y * p1z);
                                }
                                else
                                {
                                    o2x = (a1bx * p1y * p2x - node_current.x * p2y * p1x - a1by * p1x * p2x
                                        + node_current.y * p1x * p2x) / (p1y * p2x - p2y * p1x);

                                    o2y = (a1by * p1x * p2y - node_current.y * p2x * p1y - a1bx * p1y * p2y
                                        + node_current.x * p1y * p2y) / (p1x * p2y - p2x * p1y);

                                    if (p1y * p2z - p2y * p1z == 0)
                                    {
                                        o2z = (a1bz * p1x * p2z - node_current.z * p2x * p1z - a1bx * p1z * p2z
                                        + node_current.x * p1z * p2z) / (p1x * p2z - p2x * p1z);
                                    }
                                    else
                                    {
                                        o2z = (a1bz * p1y * p2z - node_current.z * p2y * p1z - a1by * p1z * p2z
                                        + node_current.y * p1z * p2z) / (p1y * p2z - p2y * p1z);
                                    }
                                }

                                float t2 = (-dpl_compare - apl_compare * o2x - bpl_compare * o2y
                                    - cpl_compare * o2z) / (Mathf.Pow(apl_compare, 2) + Mathf.Pow(bpl_compare, 2)
                                    + Mathf.Pow(cpl_compare, 2));

                                float o1x = o2x + t2 * apl_compare;
                                float o1y = o2y + t2 * bpl_compare;
                                float o1z = o2z + t2 * cpl_compare;

                                // Расстояние между плоскостями
                                float h = Vector3.Distance(new Vector3(o1x, o1y, o1z), new Vector3(o2x, o2y, o2z));

                                float x1 = Vector3.Distance(new Vector3(o1x, o1y, o1z), wire_compare.nodes[n_compare]);
                                float x2 = Vector3.Distance(new Vector3(o1x, o1y, o1z), wire_compare.nodes[n_compare + 1]);

                                float y1 = Vector3.Distance(new Vector3(o2x, o2y, o2z), wire_current.nodes[n_current]);
                                float y2 = Vector3.Distance(new Vector3(o2x, o2y, o2z), wire_current.nodes[n_current + 1]);

                                if (Math.Round(Mathf.Abs(length_segment_compare - x2) - x1, 3) != 0)
                                    x2 = -x2;

                                x1 = x2 - length_segment_compare;

                                if (Math.Round(Mathf.Abs(length_segment_current - y2) - y1, 3) != 0)
                                    y2 = -y2;

                                y1 = y2 - length_segment_current;

                                float fi = Mathf.Acos(cos_segment);
                                float AM = 0;

                                if (h != 0)
                                    AM = Mathf.Atan((x1 + y1 + pp_segments["A1A2"]) / h * Mathf.Tan(fi / 2f))
                                        + Mathf.Atan((x2 + y2 + pp_segments["B1B2"]) / h * Mathf.Tan(fi / 2f))
                                        - Mathf.Atan((x1 + y2 + pp_segments["A1B2"]) / h * Mathf.Tan(fi / 2f))
                                        - Mathf.Atan((x2 + y1 + pp_segments["B1A2"]) / h * Mathf.Tan(fi / 2f));
                                else
                                    AM = 0;

                                M[ind_m].Add((float)(2f * NU / 4f * Mathf.PI * cos_segment
                                    * (x2 * Math.Tanh(length_segment_current / (pp_segments["B1B2"] + pp_segments["B1A2"]))
                                    + y2 * Math.Tanh(length_segment_compare / (pp_segments["B1B2"] + pp_segments["A1B2"]))
                                    - x1 * Math.Tanh(length_segment_current / (pp_segments["A1A2"] + pp_segments["A1B2"]))
                                    - y1 * Math.Tanh(length_segment_compare / (pp_segments["A1A2"] + pp_segments["B1A2"]))
                                    + h / Mathf.Sin(fi) * AM)));

                                M[ind_m].WireA = wire_compare.Wire;
                                M[ind_m].WireB = wire_current.Wire;
                            }
                        }
                    }

                    ind_m += 1;
                }
            }

            return M;
        }
    }
}