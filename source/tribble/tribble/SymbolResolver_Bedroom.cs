using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    class SymbolResolver_Bedroom : SymbolResolver
    {
        public override void Resolve(ResolveParams rp)
        {
            ThingDef thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
            TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef);
            rp.wallStuff = thingDef;
            rp.singlePawnKindDef = (rp.singlePawnKindDef ?? PawnKindDef.Named("Pirate"));
            rp.singlePawnKindDef.combatPower = 250f;
            ResolveParams rpInterior = rp;
            rpInterior.rect = rp.rect.ContractedBy(1);
            List<RoomOutline> roomOutlines = new List<RoomOutline>();
            roomOutlines.Add(new RoomOutline(rp.rect));
            this.AddRoomCentersToRootsToUnfog(roomOutlines);
            BaseGen.symbolStack.Push("heatingCooling", rpInterior);
            BaseGen.symbolStack.Push("pawn", rpInterior);
            BaseGen.symbolStack.Push("doors", rp);
            BaseGen.symbolStack.Push("edgeWalls", rp);
            BaseGen.symbolStack.Push("roof", rp);
            BaseGen.symbolStack.Push("floor", rp);
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
