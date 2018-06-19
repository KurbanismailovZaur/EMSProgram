using EMSP.Communication;
using EMSP.Mathematic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mynamespace
{
	public class Mathematic3Calculator : MathematicCalculatorBase
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public float Calculate()
        {
            // Исправить это
            float t = 0f;

            Wiring.Factory wiringFactory = new Wiring.Factory();

            Wiring wiring = wiringFactory.Create();

            #region Create wiring
            Wire wire0 = wiring.CreateWire("K0", 0, 300000000, 1);
            wire0.Add(1f, 2f, 3f);
            wire0.Add(7f, 4f, 7f);
            wire0.Add(5f, 0f, 3f);
            wire0.Add(6f, 9f, 4f);
            wire0.Add(0f, 5f, 9f);
            wire0.Add(2f, 5f, 3f);
            wire0.Add(3f, 5f, 2f);
            wire0.Add(4f, 3f, 1f);

            Wire wire1 = wiring.CreateWire("K1", 0, 9000, 2);
            wire1.Add(1f, 0f, 0f);
            wire1.Add(8f, 9f, 1f);
            wire1.Add(0f, 6f, 0f);
            wire1.Add(9f, 3f, 1f);
            wire1.Add(0f, 2f, 5f);
            wire1.Add(8f, 7f, 1f);
            wire1.Add(0f, 9f, 4f);
            wire1.Add(2f, 9f, 0f);

            Wire wire2 = wiring.CreateWire("K2", 0, 200000, 0.5f);
            wire2.Add(2f, 0f, 0f);
            wire2.Add(6f, 9f, 3f);
            wire2.Add(9f, 7f, 6f);
            wire2.Add(7f, 0f, 4f);
            wire2.Add(9f, 3f, 2f);
            wire2.Add(1f, 7f, 4f);
            wire2.Add(3f, 4f, 4f);
            wire2.Add(4f, 7f, 7f);

            Wire wire3 = wiring.CreateWire("K3", 0, 1000000, -1);
            wire3.Add(3f, 0f, 0f);
            wire3.Add(8f, 2f, 6f);
            wire3.Add(8f, 2f, 0f);
            wire3.Add(9f, 1f, 8f);
            wire3.Add(1f, 9f, 0f);
            wire3.Add(9f, 8f, 4f);
            wire3.Add(4f, 8f, 8f);
            wire3.Add(8f, 7f, 7f);

            Wire wire4 = wiring.CreateWire("K4", 0, 500000, -1.5f);
            wire4.Add(3f, 0f, 0f);
            wire4.Add(18f, 12f, 16f);
            wire4.Add(18f, 12f, 10f);
            wire4.Add(19f, 11f, 18f);
            wire4.Add(11f, 19f, 10f);
            wire4.Add(19f, 18f, 14f);
            wire4.Add(14f, 18f, 18f);
            wire4.Add(18f, 17f, 17f);
            #endregion

            // Угловая частота
            float[] w = new float[wiring.Count];

            // Угловая частота
            float[] Z = new float[wiring.Count];

            // Итоговая напряженность по базовым координатам
            float Ex = 0;
            float Ey = 0;
            float Ez = 0;

            for (int i = 0; i < wiring.Count; i++)
            {
                // Вычисление угловой частоты и границы зоны
                w[i] = wiring[i].Frequency * 2f * Mathf.PI;
                Z[i] = CC / w[i];

                // Максимальное значение для количества узлов
                int jointCountMinusOne = wiring[i].Count - 1;

                #region Declarations
                // Вектор прямой
                float[,,] Pxyz = new float[wiring.Count, jointCountMinusOne, 3];

                // r0 - rc
                float[,,] r0rC = new float[wiring.Count, jointCountMinusOne, 3];

                // r0 - rc * p
                float[,] r0rC_p = new float[wiring.Count, jointCountMinusOne];

                // p*p
                float[,] pp = new float[wiring.Count, jointCountMinusOne];

                // rb
                float[,,] rb = new float[wiring.Count, jointCountMinusOne, 3];

                // Пределы a b
                float[,,] ab = new float[wiring.Count, jointCountMinusOne, 2];

                // Проекция C
                float[,,] nh = new float[wiring.Count, jointCountMinusOne, 2];

                // Искомые элементы напряженности
                float[,] Erx = new float[wiring.Count, jointCountMinusOne];
                float[,] Ery = new float[wiring.Count, jointCountMinusOne];
                float[,] Ettx = new float[wiring.Count, jointCountMinusOne];
                float[,] Etty = new float[wiring.Count, jointCountMinusOne];
                float[,] Exb = new float[wiring.Count, jointCountMinusOne];
                float[,] Eyb = new float[wiring.Count, jointCountMinusOne];
                float[,] Eb = new float[wiring.Count, jointCountMinusOne];
                float[,] Exd = new float[wiring.Count, jointCountMinusOne];
                float[,] Eyd = new float[wiring.Count, jointCountMinusOne];
                float[,] Ed = new float[wiring.Count, jointCountMinusOne];

                // Переход от локальных к базовым координатам
                float[,,] Lbx = new float[wiring.Count, jointCountMinusOne, 3];
                float[,,] Lby = new float[wiring.Count, jointCountMinusOne, 3];

                // Финальные значения по координатам
                float[,,] Ec = new float[wiring.Count, jointCountMinusOne, 3];

                // Финальные значения по абсолюту
                float[,] E = new float[wiring.Count, jointCountMinusOne];

                Vector3 C = new Vector3(6f, 1f, 0f);

                int iterations = 100;
                #endregion

                for (int j = 0; j < jointCountMinusOne; j++)
                {
                    // Вычисление вектора прямой
                    Pxyz[i, j, 0] = wiring[i][j + 1].x - wiring[i][j].x;
                    Pxyz[i, j, 1] = wiring[i][j + 1].y - wiring[i][j].y;
                    Pxyz[i, j, 2] = wiring[i][j + 1].z - wiring[i][j].z;

                    // Вычисление r0 - r1
                    r0rC[i, j, 0] = wiring[i][j].x - C.x;
                    r0rC[i, j, 1] = wiring[i][j].y - C.y;
                    r0rC[i, j, 2] = wiring[i][j].z - C.z;

                    // Вычисление (r0-rC)*p
                    r0rC_p[i, j] = r0rC[i, j, 0] * Pxyz[i, j, 0] + r0rC[i, j, 1] * Pxyz[i, j, 1] + r0rC[i, j, 2] * Pxyz[i, j, 2];

                    // Вычисление p*p
                    pp[i, j] = Mathf.Pow(Pxyz[i, j, 0], 2f) + Mathf.Pow(Pxyz[i, j, 1], 2f) + Mathf.Pow(Pxyz[i, j, 2], 2f);

                    // Вычисление rb
                    rb[i, j, 0] = wiring[i][j].x - r0rC_p[i, j] / pp[i, j] * Pxyz[i, j, 0];
                    rb[i, j, 1] = wiring[i][j].y - r0rC_p[i, j] / pp[i, j] * Pxyz[i, j, 1];
                    rb[i, j, 2] = wiring[i][j].z - r0rC_p[i, j] / pp[i, j] * Pxyz[i, j, 2];

                    // Вычисление пределов a b
                    if (j == 0)
                    {
                        ab[i, j, 0] = 0;
                        ab[i, j, 1] = Mathf.Sqrt(pp[i, j]);
                    }
                    else
                    {
                        ab[i, j, 0] = ab[i, j - 1, 1];
                        ab[i, j, 1] = Mathf.Sqrt(pp[i, j]) + ab[i, j, 0];
                    }

                    // Вычисление проекции С
                    nh[i, j, 0] = Mathf.Sqrt(Mathf.Pow(rb[i, j, 0] - wiring[i][j].x, 2f) + Mathf.Pow(rb[i, j, 1] - wiring[i][j].y, 2f) + Mathf.Pow(rb[i, j, 2] - wiring[i][j].z, 2f)) + ab[i, j, 0];
                    nh[i, j, 1] = Mathf.Sqrt(Mathf.Pow(C.x - rb[i, j, 0], 2f) + Mathf.Pow(C.y - rb[i, j, 1], 2f) + Mathf.Pow(C.z - rb[i, j, 2], 2f));

                    // Алгоритм
                    float dx = (ab[i, j, 1] - ab[i, j, 0]) / iterations;

                    Erx[i, j] = 0;
                    Ery[i, j] = 0;
                    Ettx[i, j] = 0;
                    Exd[i, j] = 0;
                    Eyd[i, j] = 0;

                    for (int it = 0; it < iterations; it++)
                    {
                        float x = it * dx + dx / 2f + ab[i, j, 0];
                        float bx = nh[i, j, 0] - x;
                        float r = Mathf.Sqrt(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[i, j, 1], 2f));
                        float R3 = Mathf.Pow(r, 3f);
                        float RR3 = 1f / R3;

                        float costt = bx / r;
                        float tt = Mathf.Acos(costt);

                        float dq;
                        if (it == 0)
                            dq = -wiring[i].Amperage / w[i] * (Mathf.Cos(w[i] * (t - (ab[i, j, 0] + dx) / CC)) - Mathf.Cos(w[i] * (t - ab[i, j, 0] / CC)));
                        else
                            dq = -wiring[i].Amperage / w[i] * (Mathf.Cos(w[i] * (t - (ab[i, j, 0] + dx * (it + 1)) / CC)) - Mathf.Cos(w[i] * (t - (ab[i, j, 0] + dx * it) / CC)));

                        Erx[i, j] += (Mathf.Pow(bx, 2f) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[i, j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq;
                        Ery[i, j] += ((bx * nh[i, j, 1]) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[i, j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq;
                        Ettx[i, j] -= (Mathf.Pow(nh[i, j, 1], 2f) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[i, j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq / 2f;
                        Exd[i, j] += wiring[i].Amperage * w[i] * dx * Mathf.Sin(w[i] * (t - r / CC)) / Mathf.Pow(CC, 2f) / r / 4f / Mathf.PI / EPS * Mathf.Pow(Mathf.Sin(tt), 2f);
                        Eyd[i, j] += wiring[i].Amperage * w[i] * dx * Mathf.Sin(w[i] * (t - r / CC)) / Mathf.Pow(CC, 2f) / r / 4f / Mathf.PI / EPS * Mathf.Sin(tt) * costt;
                    }

                    Etty[i, j] = -Ery[i, j] / 2f;
                    Exb[i, j] = Erx[i, j] + Ettx[i, j];
                    Eyb[i, j] = Ery[i, j] + Etty[i, j];
                    Eb[i, j] = Mathf.Sqrt(Mathf.Pow(Exb[i, j], 2f) + Mathf.Pow(Eyb[i, j], 2f));
                    Ed[i, j] = Mathf.Sqrt(Mathf.Pow(Exd[i, j], 2f) + Mathf.Pow(Eyd[i, j], 2f));

                    // Переход от локальных к базовым координатам
                    float Rs = 0f;

                    for (int k = 0; k < 3; k++)
                    {
                        Lbx[i, j, k] = Pxyz[i, j, k] / (ab[i, j, 1] - ab[i, j, 0]);
                        Lby[i, j, k] = (GetVectorValue(C, k) - rb[i, j, k]) / nh[i, j, 1];

                        // Среднее растояние для зоны
                        Rs += Mathf.Pow(GetVectorValue(C, k) - (GetVectorValue(wiring[i][j + 1], k)) / 2f, 2f);
                    }

                    Rs = Mathf.Sqrt(Rs);
                    E[i, j] = 0f;

                    for (int k = 0; k < 3; k++)
                    {
                        if (Rs < Z[i] / KZ)
                            Ec[i, j, k] = Exb[i, j] * Lbx[i, j, k] + Eyb[i, j] * Lby[i, j, k];
                        else if (Rs > Z[i] * KZ)
                            Ec[i, j, k] = Exd[i, j] * Lbx[i, j, k] + Eyd[i, j] * Lby[i, j, k];
                        else
                            Ec[i, j, k] = (Exb[i, j] + Exd[i, j]) * Lbx[i, j, k] + (Eyb[i, j] + Eyd[i, j]) * Lby[i, j, k];

                        E[i, j] += Mathf.Pow(Ec[i, j, k], 2f);
                    }

                    E[i, j] = Mathf.Sqrt(E[i, j]);

                    Ex += Ec[i, j, 0];
                    Ey += Ec[i, j, 1];
                    Ez += Ec[i, j, 2];
                }
            }

            float EE = Mathf.Sqrt(Mathf.Pow(Ex, 2f) + Mathf.Pow(Ey, 2f) + Mathf.Pow(Ez, 2f));

            return EE;
        }

        private float GetVectorValue(Vector3 vector3, int index)
        {
            return index == 0 ? vector3.x : index == 1 ? vector3.y : vector3.z;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }

    public class Mathematic3 : MonoBehaviour
    {
        private void Start()
        {
            Mathematic3Calculator math3 = new Mathematic3Calculator();
            Debug.Log(math3.Calculate());
        }
    }
}
