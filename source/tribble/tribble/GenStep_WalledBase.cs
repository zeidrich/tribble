using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    public class GenStep_WalledBase : GenStep_Scatterer
    {
        private static readonly IntRange FactionBaseSizeRange = new IntRange(34, 35);

        protected override bool CanScatterAt(IntVec3 c, Map map)
        {
            return base.CanScatterAt(c, map) && c.Standable(map) && !c.Roofed(map) && map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
        }

        protected override void ScatterAt(IntVec3 c, Map map, int stackCount = 1)
        {
            int randomInRange = FactionBaseSizeRange.RandomInRange;
            int randomInRange2 = FactionBaseSizeRange.RandomInRange;
            CellRect rect = new CellRect(c.x - 19, c.z - 19, 39, 39);
            Faction faction;
            if (map.info.parent == null || map.info.parent.Faction == null || map.info.parent.Faction == Faction.OfPlayer)
            {
                faction = Find.FactionManager.RandomEnemyFaction(false, false);
            }
            else
            {
                faction = map.info.parent.Faction;
            }
            if (FactionBaseSymbolResolverUtility.ShouldUseSandbags(faction))
            {
               // rect = rect.ExpandedBy(4);
            }
            rect.ClipInsideMap(map);
            ResolveParams resolveParams = default(ResolveParams);
            resolveParams.rect = rect;
            resolveParams.faction = faction;
            BaseGen.globalSettings.map = map;
            BaseGen.symbolStack.Push("factionBase", resolveParams);
            //BaseGen.symbolStack.Push("farm", resolveParams);
            BaseGen.Generate();
        }
    }
}
