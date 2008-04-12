using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GammaDraconis.Types
{
    /// <summary>
    /// A point of colored light.
    /// Based heavily upon PointLight.cs from Microsoft's Materials and Lights example.
    /// </summary>
    class PointLight : GameObject
    {
        private EffectParameter positionParameter;
        private EffectParameter instanceParameter;
        private float rangeValue = 30f;
        private float falloffValue = 2f;
        private Color colorValue = Color.White;

        public PointLight(EffectParameter lightParameter)
            : base()
        {
            instanceParameter = lightParameter;
            positionParameter = instanceParameter.StructureMembers["position"];
            instanceParameter.StructureMembers["range"].SetValue(rangeValue);
            instanceParameter.StructureMembers["falloff"].SetValue(falloffValue);
            instanceParameter.StructureMembers["color"].SetValue(
                colorValue.ToVector4());
        }

        #region Light Properties
        public Coords Position
        {
            set
            {
                position = value;
                positionParameter.SetValue(new Vector4(position.pos(), 1));
            }
            get
            {
                return position;
            }
        }


        public Color Color
        {
            set
            {
                colorValue = value;
                instanceParameter.StructureMembers["color"].SetValue(
                    colorValue.ToVector4());
            }
            get
            {
                return colorValue;
            }
        }

        public float Range
        {
            set
            {
                rangeValue = value;
                instanceParameter.StructureMembers["range"].SetValue(rangeValue);
            }
            get
            {
                return rangeValue;
            }
        }


        public float Falloff
        {
            set
            {
                falloffValue = value;
                instanceParameter.StructureMembers["falloff"].SetValue(falloffValue);
            }
            get
            {
                return falloffValue;
            }
        }
        #endregion
    }
}
