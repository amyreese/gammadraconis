using System;
using System.Collections.Generic;
using System.Text;
using LuaInterface;

namespace GammaDraconis.Core
{
    class GameLua : Lua
    {

        public GameLua() : base()
        {
            #region Lua initialization of C# classes
            String initCode = @" 

using = luanet.load_assembly
import = luanet.import_type

using('Microsoft.Xna.Framework')

MSMath = import('System.Math')
Random = import('System.Random')
MathHelper = import('Microsoft.Xna.Framework.MathHelper')
Matrix = import('Microsoft.Xna.Framework.Matrix')
Rectangle = import('Microsoft.Xna.Framework.Rectangle')
Vector2 = import('Microsoft.Xna.Framework.Vector2')
Vector3 = import('Microsoft.Xna.Framework.Vector3')
Color = import('Microsoft.Xna.Framework.Graphics.Color')

using('GammaDraconis');

GammaDraconis = import('GammaDraconis.GammaDraconis')
GammaDraconis = GammaDraconis.GetInstance()
            
Interface = import('GammaDraconis.Video.GUI.Interface')
Sprite = import('GammaDraconis.Video.GUI.Sprite')
Text = import('GammaDraconis.Video.GUI.Text')
            ";

            DoLua(initCode);
            #endregion

            #region Lua helper functions
            String funcCode = @"
Libraries = {}
function library( filename )
    if ( Libraries.filename == nil ) then
        dofile('Lua/' .. filename .. '.lua')
    end
end

function include( filename )
    dofile(mapPath .. filename)
end
            ";
            DoLua( funcCode );
            #endregion

            /* Register Function Sample
            RegisterFunction("RegisterFunction", "RegisterFunction", this, new Type[] { typeof(String), typeof(String), typeof(Object) });
            SetMap("");
            Call("RegisterFunction", new object[] { "SetMap", "SetMap", this });
            Call("SetMap", "test");
             */
        }

        public void SetMap( string mapName )
        {
            String mapPath = (mapName.Length > 0) ? ("Maps/" + mapName + "/") : ("");
            String mapCode = "mapPath = '" + mapPath + "'";
            DoLua(mapCode);
        }

        private void DoLua( string lua )
        {
            try
            {
                DoString(lua);
            }
            catch (LuaException e)
            {
                GammaDraconis.debug(e.Message);
            }
        }

        public object[] Call(string function, params object[] args)
        {
            LuaFunction luaFunction = GetFunction(function);
            if (luaFunction == null)
            {
                throw new ArgumentException("Function '" + function + "' does not seem to exist in the Lua");
            }
            return luaFunction.Call(args);
        }

        public void RegisterFunction(String luaFunctionPath, String cFunctionName, Object objectInstance)
        {
            base.RegisterFunction(luaFunctionPath, objectInstance, objectInstance.GetType().GetMethod(cFunctionName));
        }

        public void RegisterFunction(String luaFunctionPath, String cFunctionName, Object objectInstance, Type[] types)
        {
            base.RegisterFunction(luaFunctionPath, objectInstance, objectInstance.GetType().GetMethod(cFunctionName,types));
        }
    }
}
