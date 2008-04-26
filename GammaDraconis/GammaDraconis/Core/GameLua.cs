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

using('System')
Console = import('System.Console')
print = Console.WriteLine

using('Microsoft.Xna.Framework')

MSMath = import('System.Math')
Random = import('System.Random')
MathHelper = import('Microsoft.Xna.Framework.MathHelper')
Matrix = import('Microsoft.Xna.Framework.Matrix')
Rectangle = import('Microsoft.Xna.Framework.Rectangle')
Vector2 = import('Microsoft.Xna.Framework.Vector2')
Vector3 = import('Microsoft.Xna.Framework.Vector3')
BoundingBox = import('Microsoft.Xna.Framework.BoundingBox')
Color = import('Microsoft.Xna.Framework.Graphics.Color')
PlayerIndex = import('Microsoft.Xna.Framework.PlayerIndex')
using('GammaDraconis');

GammaDraconis = import('GammaDraconis.GammaDraconis')
GammaDraconis = GammaDraconis.GetInstance()
            
Interface = import('GammaDraconis.Video.GUI.Interface')
Sprite = import('GammaDraconis.Video.GUI.Sprite')
Text = import('GammaDraconis.Video.GUI.Text')

Bullet = import('GammaDraconis.Types.Bullet')
Checkpoint = import('GammaDraconis.Types.Checkpoint')
Skybox = import('GammaDraconis.Types.Skybox')
Coords = import('GammaDraconis.Types.Coords')
Course = import('GammaDraconis.Types.Course')
GameObject = import('GammaDraconis.Types.GameObject')
MountPoint = import('GammaDraconis.Types.MountPoint')
Player = import('GammaDraconis.Types.Player')
Racer = import('GammaDraconis.Types.Racer')
Turret = import('GammaDraconis.Types.Turret')
Weapon = import('GammaDraconis.Types.Weapon')

Input = import('GammaDraconis.Core.Input.Input')

Proto = import('GammaDraconis.Core.Proto')
Engine = import('GammaDraconis.Core.Engine')
Race = import('GammaDraconis.Core.Race')

GO_TYPE = import('GammaDraconis.Video.GO_TYPE')
FBXModel = import('GammaDraconis.Video.FBXModel')
Scene = import('GammaDraconis.Video.Scene')
Room = import('GammaDraconis.Video.Room')
            ";

            DoString(initCode);
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
    dofile(mapPath .. filename ..'.lua')
end
            ";
            DoString( funcCode );
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
            DoString(mapCode);
        }

        public void LoadMap(string mapName)
        {
            SetMap(mapName);
            DoString("include( '" + mapName + "' )");
        }

        public object[] Call(string function, params object[] args)
        {
            LuaFunction luaFunction = GetFunction(function);
            if (luaFunction == null)
            {
                throw new ArgumentException("Function '" + function + "' does not seem to exist in the Lua");
            }
            try
            {
                return luaFunction.Call(args);
            }
            catch (LuaException e)
            {
                Console.WriteLine("Lua died!  F(): " + function);
                return null;
            }
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
