using EMSP.Communication;
using EMSP.Utility.Extensions;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EMSP.Mathematic
{
    public class MathematicManager : MonoSingleton<MathematicManager>
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        private struct MagneticTensionsInfo
        {
            public Vector3 point;

            public float magneticTensions;
        }
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [Header("Common")]
        [SerializeField]
        [Range(8, 128)]
        private int _rangeLength = 16;

        [SerializeField]
        private float _amperage = 2f;

        private Mathematic1 _math1 = new Mathematic1();

        [Header("Magnetic Tensions In Space")]
        [SerializeField]
        private Material _magneticTensionMaterial;

        [SerializeField]
        private Transform _magneticTensionsInSpace;

        [SerializeField]
        [Range(0f, 1f)]
        private float _stretchPercent = 0f;

        [SerializeField]
        [Range(0f, 1f)]
        private float _pointSizeStretchPercent = 0f;

        [SerializeField]
        [Range(0f, 1f)]
        private float _ignoreMagneticTensionWithAlphaLess = 0f;

        private MagneticTensionsInfo[] _magneticTensionsInfo;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void Calculate(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.MagneticTensionInSpace:
                    CalculateMagneticTensionInSpace();
                    break;
            }
        }

        private void CalculateMagneticTensionInSpace()
        {
            Bounds wiringBounds = WiringManager.Instance.Wiring.GetBounds();
            float maxSide = GetBoundsMaxSide(wiringBounds);
            float stretchedMaxSide = maxSide + (maxSide * _stretchPercent);

            float step = stretchedMaxSide / (_rangeLength - 1);

            _magneticTensionsInfo = new MagneticTensionsInfo[(int)Mathf.Pow(_rangeLength, 3)];
            int index = 0;

            for (int i = 0; i < _rangeLength; i++)
            {
                for (int j = 0; j < _rangeLength; j++)
                {
                    for (int k = 0; k < _rangeLength; k++)
                    {
                        Vector3 point = wiringBounds.center - (new Vector3(stretchedMaxSide, stretchedMaxSide, stretchedMaxSide) / 2f) + (new Vector3(i, j, k) * step);
                        float magneticTension = _math1.Calculate(WiringManager.Instance.Wiring, point, _amperage);

                        _magneticTensionsInfo[index++] = new MagneticTensionsInfo
                        {
                            point = point,
                            magneticTensions = magneticTension
                        };
                    }
                }
            }

            float maxMagneticTension = _magneticTensionsInfo.Max(x => x.magneticTensions);

            foreach (MagneticTensionsInfo mtInfo in _magneticTensionsInfo)
            {
                float alpha = Remap(mtInfo.magneticTensions, 0f, maxMagneticTension, 0f, 1f);
                Transform magneticTensionTransform = CreateMagneticTensionInSpace(PrimitiveType.Sphere, mtInfo.point, step + (step * _pointSizeStretchPercent), alpha);

                if (!magneticTensionTransform)
                {
                    continue;
                }

                magneticTensionTransform.SetParent(_magneticTensionsInSpace);
            }
        }

        private float GetBoundsMaxSide(Bounds bounds)
        {
            Vector3 size = bounds.size;
            return Mathf.Max(size.x, size.y, size.z);
        }

        private Transform CreateMagneticTensionInSpace(PrimitiveType primitiveType, Vector3 point, float size, float alpha)
        {
            if (alpha < _ignoreMagneticTensionWithAlphaLess)
            {
                return null;
            }

            Transform magneticTensionObject = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            magneticTensionObject.position = point;
            magneticTensionObject.localScale = new Vector3(size, size, size);

            Renderer magneticTensionRenderer = magneticTensionObject.GetComponent<Renderer>();

            Material material = new Material(_magneticTensionMaterial);
            material.SetAlpha(alpha);

            magneticTensionRenderer.material = material;

            return magneticTensionObject;
        }

        private float Remap(float value, float iMin, float iMax, float oMin, float oMax)
        {
            return oMin + (value - iMin) * (oMax - oMin) / (iMax - iMin);
        }

        public void ShowCalculatedResult(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.MagneticTensionInSpace:
                    _magneticTensionsInSpace.gameObject.SetActive(true);
                    break;
            }
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}