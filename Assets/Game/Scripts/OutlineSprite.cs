using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts
{
    public class OutlineSprite : MonoBehaviour
    {
        private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private const string OutlineEnabled = "_OUTLINEENABLED";

        [SerializeField] private OutlineSpriteAsset outlinePreset;
        [SerializeField] private SpriteRenderer[] m_sprites;

        private Material[] m_materials;
        private LocalKeyword[] m_keywords;
        
        public void Toggle(bool value)
        {
            if (outlinePreset == null) return;
            
            for (var i = 0; i < m_materials.Length; i++)
            {
                m_materials[i].SetFloat(OutlineThickness, outlinePreset.GetThickness());
                m_materials[i].SetColor(OutlineColor, outlinePreset.GetColor());
                m_materials[i].SetKeyword(m_keywords[i], value);
            }
        }

        private void Awake()
        {
            m_materials = new Material[m_sprites.Length];
            m_keywords = new LocalKeyword[m_sprites.Length];

            for (var i = 0; i < m_sprites.Length; i++)
            {
                m_materials[i] = m_sprites[i].material;
                m_keywords[i] = m_materials[i].shader.keywordSpace.FindKeyword(OutlineEnabled);
            }
        }
    }
}
