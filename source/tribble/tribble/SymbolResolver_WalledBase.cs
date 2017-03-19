using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    public class SymbolResolver_WalledBase : SymbolResolver
    {
        private static readonly FloatRange NeolithicPawnsPoints = new FloatRange(825f, 1320f);

        private static readonly FloatRange NonNeolithicPawnsPoints = new FloatRange(1320f, 1980f);

        public override void Resolve(ResolveParams rp)
        {
            Map map = BaseGen.globalSettings.map;
            Faction faction = rp.faction ?? Find.FactionManager.RandomEnemyFaction(false, false);
            CellRect cellRect = rp.rect;

            List<RoomOutline> roomOutlines = new List<RoomOutline>();
            roomOutlines.Add(new RoomOutline(rp.rect));

            /* Generate Pawns from SymbolResolver_FactionBase*/

                /*
                CellRect rect = rp.rect;
                Lord singlePawnLord = rp.singlePawnLord ?? LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rp.rect.CenterCell), map, null);
                ResolveParams resolveParams = rp;
                resolveParams.rect = rect;
                resolveParams.faction = faction;
                resolveParams.singlePawnLord = singlePawnLord;
                resolveParams.pawnGroupKindDef = (rp.pawnGroupKindDef ?? PawnGroupKindDefOf.FactionBase);
                resolveParams.singlePawnSpawnCellExtraPredicate = (rp.singlePawnSpawnCellExtraPredicate ?? ((IntVec3 x) => this.CanReachAnyRoom(x, roomOutlines)));
            

                if (resolveParams.pawnGroupMakerParams == null)
                {
                    float points = (!faction.def.techLevel.IsNeolithicOrWorse()) ? NonNeolithicPawnsPoints.RandomInRange : NeolithicPawnsPoints.RandomInRange;
                    resolveParams.pawnGroupMakerParams = new PawnGroupMakerParms();
                    resolveParams.pawnGroupMakerParams.map = map;
                    resolveParams.pawnGroupMakerParams.faction = faction;
                    resolveParams.pawnGroupMakerParams.points = points;
                }
                BaseGen.symbolStack.Push("pawnGroup", resolveParams);
                */

            this.AddRoomCentersToRootsToUnfog(roomOutlines);
            BaseGen.symbolStack.Push("fillWithCells", rp);
            BaseGen.symbolStack.Push("floor", rp);
            BaseGen.symbolStack.Push("doors", rp);
            BaseGen.symbolStack.Push("unflooredEmpty", rp);
        }

        private bool CanReachAnyRoom(IntVec3 root, List<RoomOutline> allRooms)
        {
            Map map = BaseGen.globalSettings.map;
            for (int i = 0; i < allRooms.Count; i++)
            {
                if (map.reachability.CanReach(root, allRooms[i].rect.RandomCell, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
                {
                    return true;
                }
            }
            return false;
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
