using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP
{
    public class Model : MonoBehaviour
    {
        #region Entities
        #region Enums
        public enum RenderMode
        {
            Opaque,
            Fade
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class Factory
        {
            public Model MakeFactory(GameObject obj)
            {
                Model model = obj.AddComponent<Model>();
                Renderer[] renderers = model.GetComponentsInChildren<Renderer>();

                List<Material> uniqueMaterials = new List<Material>();
                foreach (Renderer renderer in renderers)
                {
                    foreach (Material material in renderer.sharedMaterials)
                    {
                        if (!uniqueMaterials.Contains(material))
                        {
                            uniqueMaterials.Add(material);
                        }
                    }
                }

                model._sharedMaterials = uniqueMaterials.ToArray();

                return model;
            }
        }

        [Serializable]
        public class TransparentStateChangedEvent : UnityEvent<Model, bool> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private bool _isTransparent;

        private Material[] _sharedMaterials;
        #endregion

        #region Events
        public TransparentStateChangedEvent TransparentStateChanged = new TransparentStateChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public bool IsTransparent
        {
            get { return _isTransparent; }
            set
            {
                if (_isTransparent == value)
                {
                    return;
                }

                SwitchMaterialsRenderModeTo(value ? RenderMode.Fade : RenderMode.Opaque);

                _isTransparent = value;

                TransparentStateChanged.Invoke(this, _isTransparent);
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void MakeMaterialTransparent(Material material)
        {
            material.SetFloat("_Mode", 2);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            Color color = material.color;
            color.a = 0.125f;
            material.color = color;
        }

        private void MakeMaterialOpaque(Material material)
        {
            material.SetFloat("_Mode", 0);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 2000;

            Color color = material.color;
            color.a = 1;
            material.color = color;
        }

        private void SwitchMaterialsRenderModeTo(RenderMode renderMode)
        {
            if (renderMode == RenderMode.Opaque)
            {
                foreach (Material material in _sharedMaterials)
                {
                    MakeMaterialOpaque(material);
                }
            }
            else
            {
                foreach (Material material in _sharedMaterials)
                {
                    MakeMaterialTransparent(material);
                }
            }
        }

        private void OnDestroy()
        {
            List<Texture> textures = new List<Texture>();

            foreach (Material material in _sharedMaterials)
            {
                if (material.mainTexture)
                {
                    textures.Add(material.mainTexture);
                }

                Texture normalMap = material.GetTexture("_BumpMap");
                if (normalMap)
                {
                    textures.Add(normalMap);
                }
            }

            foreach (Texture texture in textures)
            {
                Destroy(texture);
            }

            foreach (Material material in _sharedMaterials)
            {
                Destroy(material);
            }

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            List<Mesh> sharedMeshes = new List<Mesh>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (!sharedMeshes.Contains(meshFilter.sharedMesh))
                {
                    sharedMeshes.Add(meshFilter.sharedMesh);
                }
            }

            foreach (Mesh mesh in sharedMeshes)
            {
                Destroy(mesh);
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