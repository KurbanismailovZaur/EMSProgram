using EMSP.Communication;
using EMSP.Mathematic;
using Numba;
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
        public override Data Calculate(Data settings)
        {
            throw new System.NotImplementedException();
        }

        public float CalculateWiring()
        {
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

            // Граница зоны
            float[] Z = new float[wiring.Count];

            // Итоговая напряженность по базовым координатам
            Vector3 exyz = Vector3.zero;

            for (int i = 0; i < wiring.Count; i++)
            {
                exyz += CalculateWire(wiring[i]);
            }

            float EE = Mathf.Sqrt(Mathf.Pow(exyz.x, 2f) + Mathf.Pow(exyz.y, 2f) + Mathf.Pow(exyz.z, 2f));

            return EE;
        }

        private Vector3 CalculateWire(Wire wire)
        {
            // Исправить это
            float t = 0f;

            // Вычисление угловой частоты и границы зоны
            float angularFrequency = wire.Frequency * 2f * Mathf.PI;

            // Вычисление границы зоны
            float zoneBoundary = CC / angularFrequency;

            // Максимальное значение для количества узлов
            int jointCountMinusOne = wire.Count - 1;

            #region Declarations
            // Вектор прямой
            float[,] Pxyz = new float[jointCountMinusOne, 3];

            // r0 - rc
            float[,] r0rC = new float[jointCountMinusOne, 3];

            // r0 - rc * p
            float[] r0rC_p = new float[jointCountMinusOne];

            // p*p
            float[] pp = new float[jointCountMinusOne];

            // rb
            float[,] rb = new float[jointCountMinusOne, 3];

            // Пределы a b
            float[,] ab = new float[jointCountMinusOne, 2];

            // Проекция C
            float[,] nh = new float[jointCountMinusOne, 2];

            // Искомые элементы напряженности
            float[] Erx = new float[jointCountMinusOne];
            float[] Ery = new float[jointCountMinusOne];
            float[] Ettx = new float[jointCountMinusOne];
            float[] Etty = new float[jointCountMinusOne];
            float[] Exb = new float[jointCountMinusOne];
            float[] Eyb = new float[jointCountMinusOne];
            float[] Eb = new float[jointCountMinusOne];
            float[] Exd = new float[jointCountMinusOne];
            float[] Eyd = new float[jointCountMinusOne];
            float[] Ed = new float[jointCountMinusOne];

            // Переход от локальных к базовым координатам
            float[,] Lbx = new float[jointCountMinusOne, 3];
            float[,] Lby = new float[jointCountMinusOne, 3];

            // Финальные значения по координатам
            float[,] Ec = new float[ jointCountMinusOne, 3];

            // Финальные значения по абсолюту
            float[] E = new float[jointCountMinusOne];

            Vector3 exyz = Vector3.zero;

            Vector3 C = new Vector3(6f, 1f, 0f);

            int iterations = 100;
            #endregion

            for (int j = 0; j < jointCountMinusOne; j++)
            {
                // Вычисление вектора прямой
                Pxyz[j, 0] = wire[j + 1].x - wire[j].x;
                Pxyz[j, 1] = wire[j + 1].y - wire[j].y;
                Pxyz[j, 2] = wire[j + 1].z - wire[j].z;

                // Вычисление r0 - r1
                r0rC[j, 0] = wire[j].x - C.x;
                r0rC[j, 1] = wire[j].y - C.y;
                r0rC[j, 2] = wire[j].z - C.z;

                // Вычисление (r0-rC)*p
                r0rC_p[j] = r0rC[j, 0] * Pxyz[j, 0] + r0rC[j, 1] * Pxyz[j, 1] + r0rC[j, 2] * Pxyz[j, 2];

                // Вычисление p*p
                pp[j] = Mathf.Pow(Pxyz[j, 0], 2f) + Mathf.Pow(Pxyz[j, 1], 2f) + Mathf.Pow(Pxyz[j, 2], 2f);

                // Вычисление rb
                rb[j, 0] = wire[j].x - r0rC_p[j] / pp[j] * Pxyz[j, 0];
                rb[j, 1] = wire[j].y - r0rC_p[j] / pp[j] * Pxyz[j, 1];
                rb[j, 2] = wire[j].z - r0rC_p[j] / pp[j] * Pxyz[j, 2];

                // Вычисление пределов a b
                if (j == 0)
                {
                    ab[j, 0] = 0;
                    ab[j, 1] = Mathf.Sqrt(pp[j]);
                }
                else
                {
                    ab[j, 0] = ab[j - 1, 1];
                    ab[j, 1] = Mathf.Sqrt(pp[j]) + ab[j, 0];
                }

                // Вычисление проекции С
                nh[j, 0] = Mathf.Sqrt(Mathf.Pow(rb[j, 0] - wire[j].x, 2f) + Mathf.Pow(rb[j, 1] - wire[j].y, 2f) + Mathf.Pow(rb[j, 2] - wire[j].z, 2f)) + ab[j, 0];
                nh[j, 1] = Mathf.Sqrt(Mathf.Pow(C.x - rb[j, 0], 2f) + Mathf.Pow(C.y - rb[j, 1], 2f) + Mathf.Pow(C.z - rb[j, 2], 2f));

                // Алгоритм
                float dx = (ab[j, 1] - ab[j, 0]) / iterations;

                Erx[j] = 0;
                Ery[j] = 0;
                Ettx[j] = 0;
                Exd[j] = 0;
                Eyd[j] = 0;

                for (int it = 0; it < iterations; it++)
                {
                    float x = it * dx + dx / 2f + ab[j, 0];
                    float bx = nh[j, 0] - x;
                    float r = Mathf.Sqrt(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[j, 1], 2f));
                    float R3 = Mathf.Pow(r, 3f);
                    float RR3 = 1f / R3;

                    float costt = bx / r;
                    float tt = Mathf.Acos(costt);

                    float dq;
                    if (it == 0)
                        dq = -wire.Amperage / angularFrequency * (Mathf.Cos(angularFrequency * (t - (ab[j, 0] + dx) / CC)) - Mathf.Cos(angularFrequency * (t - ab[j, 0] / CC)));
                    else
                        dq = -wire.Amperage / angularFrequency * (Mathf.Cos(angularFrequency * (t - (ab[j, 0] + dx * (it + 1)) / CC)) - Mathf.Cos(angularFrequency * (t - (ab[j, 0] + dx * it) / CC)));

                    Erx[j] += (Mathf.Pow(bx, 2f) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq;
                    Ery[j] += ((bx * nh[j, 1]) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq;
                    Ettx[j] -= (Mathf.Pow(nh[j, 1], 2f) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq / 2f;
                    Exd[j] += wire.Amperage * angularFrequency * dx * Mathf.Sin(angularFrequency * (t - r / CC)) / Mathf.Pow(CC, 2f) / r / 4f / Mathf.PI / EPS * Mathf.Pow(Mathf.Sin(tt), 2f);
                    Eyd[j] += wire.Amperage * angularFrequency * dx * Mathf.Sin(angularFrequency * (t - r / CC)) / Mathf.Pow(CC, 2f) / r / 4f / Mathf.PI / EPS * Mathf.Sin(tt) * costt;
                }

                Etty[j] = -Ery[j] / 2f;
                Exb[j] = Erx[j] + Ettx[j];
                Eyb[j] = Ery[j] + Etty[j];
                Eb[j] = Mathf.Sqrt(Mathf.Pow(Exb[j], 2f) + Mathf.Pow(Eyb[j], 2f));
                Ed[j] = Mathf.Sqrt(Mathf.Pow(Exd[j], 2f) + Mathf.Pow(Eyd[j], 2f));

                // Переход от локальных к базовым координатам
                float Rs = 0f;

                for (int k = 0; k < 3; k++)
                {
                    Lbx[j, k] = Pxyz[j, k] / (ab[j, 1] - ab[j, 0]);
                    Lby[j, k] = (GetVectorValue(C, k) - rb[j, k]) / nh[j, 1];

                    // Среднее растояние для зоны
                    Rs += Mathf.Pow(GetVectorValue(C, k) - (GetVectorValue(wire[j + 1], k)) / 2f, 2f);
                }

                Rs = Mathf.Sqrt(Rs);
                E[j] = 0f;

                for (int k = 0; k < 3; k++)
                {
                    if (Rs < zoneBoundary / KZ)
                        Ec[j, k] = Exb[j] * Lbx[j, k] + Eyb[j] * Lby[j, k];
                    else if (Rs > zoneBoundary * KZ)
                        Ec[j, k] = Exd[j] * Lbx[j, k] + Eyd[j] * Lby[j, k];
                    else
                        Ec[j, k] = (Exb[j] + Exd[j]) * Lbx[j, k] + (Eyb[j] + Eyd[j]) * Lby[j, k];

                    E[j] += Mathf.Pow(Ec[j, k], 2f);
                }

                E[j] = Mathf.Sqrt(E[j]);

                exyz.x += Ec[j, 0];
                exyz.y += Ec[j, 1];
                exyz.z += Ec[j, 2];
            }

            return exyz;
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
            Debug.Log(math3.CalculateWiring());
        }
    }
}
