﻿using EMSP.Communication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
    public class ElectricFieldCalculator : PointableMathematicCalculatorBase
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
        public override bool CheckIntersection(Wiring wiring, Vector3 point)
        {
            foreach (Wire wire in wiring)
            {
                for (int i = 0; i < wire.Count - 1; i++)
                {
                    Vector3 ab = wire[i + 1] - wire[i];
                    Vector3 ac = point - wire[i];

                    if (ab.normalized == ac.normalized && ac.sqrMagnitude <= ab.sqrMagnitude) return true;
                }
            }

            return false;
        }

        public override Data Calculate(Data settings)
        {
            AmperageMode amperageMode = settings.GetValue<AmperageMode>("amperageMode");
            Wiring wiring = settings.GetValue<Wiring>("wiring");
            Vector3 point = settings.GetValue<Vector3>("point");
            float time = settings.GetValue<float>("time");

            return new Data() { { "result", CalculateWiring(wiring, point, time, amperageMode) } };
        }

        public float CalculateWiring(Wiring wiring, Vector3 pointC, float time, AmperageMode amperageMode)
        {
            // Итоговая напряженность по базовым координатам
            Vector3 exyz = Vector3.zero;

            for (int i = 0; i < wiring.Count; i++)
            {
                Vector3 result = CalculateWire(wiring[i], pointC, time, amperageMode);

                if (float.IsNaN(result.x) || float.IsNaN(result.y) || float.IsNaN(result.z))
                    Debug.Log(result);

                exyz += result;
            }

            float EE = Mathf.Sqrt(Mathf.Pow(exyz.x, 2f) + Mathf.Pow(exyz.y, 2f) + Mathf.Pow(exyz.z, 2f));

            return EE;
        }

        private Vector3 CalculateWire(Wire wire, Vector3 pointC, float time, AmperageMode amperageMode)
        {
            // Вычисление силы тока.
            float amperage = amperageMode == AmperageMode.Computational ? wire.Amplitude * Mathf.Sin(2 * Mathf.PI * wire.Frequency * time) : wire.Amperage;

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
            float[,] Ec = new float[jointCountMinusOne, 3];

            // Финальные значения по абсолюту
            float[] E = new float[jointCountMinusOne];

            Vector3 exyz = Vector3.zero;

            int iterations = 100;
            #endregion

            for (int j = 0; j < jointCountMinusOne; j++)
            {
                // Вычисление вектора прямой
                Pxyz[j, 0] = wire[j + 1].x - wire[j].x;
                Pxyz[j, 1] = wire[j + 1].y - wire[j].y;
                Pxyz[j, 2] = wire[j + 1].z - wire[j].z;

                // Вычисление r0 - r1
                r0rC[j, 0] = wire[j].x - pointC.x;
                r0rC[j, 1] = wire[j].y - pointC.y;
                r0rC[j, 2] = wire[j].z - pointC.z;

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
                nh[j, 1] = Mathf.Sqrt(Mathf.Pow(pointC.x - rb[j, 0], 2f) + Mathf.Pow(pointC.y - rb[j, 1], 2f) + Mathf.Pow(pointC.z - rb[j, 2], 2f));

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
                        dq = -amperage / angularFrequency * (Mathf.Cos(angularFrequency * (time - (ab[j, 0] + dx) / CC)) - Mathf.Cos(angularFrequency * (time - ab[j, 0] / CC)));
                    else
                        dq = -amperage / angularFrequency * (Mathf.Cos(angularFrequency * (time - (ab[j, 0] + dx * (it + 1)) / CC)) - Mathf.Cos(angularFrequency * (time - (ab[j, 0] + dx * it) / CC)));

                    Erx[j] += (Mathf.Pow(bx, 2f) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq;
                    Ery[j] += ((bx * nh[j, 1]) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq;
                    Ettx[j] -= (Mathf.Pow(nh[j, 1], 2f) / Mathf.Pow(Mathf.Pow(bx, 2f) + Mathf.Pow(nh[j, 1], 2f), 2.5f)) / 2f / Mathf.PI / EPS * dq / 2f;
                    Exd[j] += amperage * angularFrequency * dx * Mathf.Sin(angularFrequency * (time - r / CC)) / Mathf.Pow(CC, 2f) / r / 4f / Mathf.PI / EPS * Mathf.Pow(Mathf.Sin(tt), 2f);
                    Eyd[j] += amperage * angularFrequency * dx * Mathf.Sin(angularFrequency * (time - r / CC)) / Mathf.Pow(CC, 2f) / r / 4f / Mathf.PI / EPS * Mathf.Sin(tt) * costt;
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

                    Lby[j, k] = (pointC[k] - rb[j, k]) / nh[j, 1];
                    if (float.IsNaN(Lby[j, k])) Lby[j, k] = 0f;

                    // Среднее растояние для зоны
                    Rs += Mathf.Pow(pointC[k] - (wire[j + 1][k]) / 2f, 2f);
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
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
