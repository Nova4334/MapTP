using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using System.Timers;

namespace MapTeleport
{
    [ApiVersion(2, 1)]
    public class MapTeleport : TerrariaPlugin
    {
        public MapTeleport(Main game) : base(game)
        {
            Order = 1;
        }
        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public override string Author
        {
            get { return "Nova4334"; }
        }
        public override string Name
        {
            get { return "MapTeleport"; }
        }
        public override string Description
        {
            get { return "Allows players to teleport to a selected location on the map."; }
        }

        public override void Initialize()
        {
            GetDataHandlers.ReadNetModule.Register(teleport);
        }

        public const string ALLOWED = "maptp";

        public const string ALLOWEDSOLIDS = "maptp.noclip";

        private void teleport(object unused, GetDataHandlers.ReadNetModuleEventArgs args)
        {
            if (args.Player.HasPermission(ALLOWED))
            {
                if (args.ModuleType == GetDataHandlers.NetModuleType.Ping)
                {
                    using (var reader = new BinaryReader(args.Data))
                    {
                        Vector2 pos = reader.ReadVector2();
                        if (!(pos.X == Tile.Type_Solid && pos.Y == Tile.Type_Solid) || args.Player.HasPermission(ALLOWEDSOLIDS))
                        {
                            args.Player.Teleport(pos.X * 16, pos.Y * 16); return;
                        }
                        else args.Player.SendErrorMessage("You are trying to teleport into a solid tile. Please choose a spot on the map that does not contain solid tiles, and try again.");
                        return;
                    }
                }
            }
        }
    }
}