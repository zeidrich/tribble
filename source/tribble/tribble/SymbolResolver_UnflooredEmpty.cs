using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    public class SymbolResolver_UnflooredEmpty : SymbolResolver
    {
        public override void Resolve(ResolveParams rp)
        {
            ThingDef thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
            TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef);
            ResolveParams resolveParams = rp;
            resolveParams.wallStuff = thingDef;
            List<RoomOutline> roomOutlines = new List<RoomOutline>();
            roomOutlines.Add(new RoomOutline(rp.rect));
            this.AddRoomCentersToRootsToUnfog(roomOutlines);
            BaseGen.symbolStack.Push("edgeWalls", resolveParams);
            ResolveParams resolveParams2 = rp;
            //BaseGen.symbolStack.Push("floor", rp);
            BaseGen.symbolStack.Push("clearForce", rp);
        }
        private void AddRoomCentersToRootsToUnfog(List<RoomOutline> allRooms)
        {
            if (Current.ProgramState != ProgramState.MapInitializing)
            {
                return;
            }
            List<IntVec3> rootsToUnfog = MapGenerator.rootsToUnfog;
            for (int i = 0; i < allRooms.Count; i++)
            {
                rootsToUnfog.Add(allRooms[i].rect.CenterCell);
            }
        }
    }
}
