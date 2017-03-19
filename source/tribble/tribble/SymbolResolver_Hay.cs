using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;


namespace tribble
{
    class SymbolResolver_Hay : SymbolResolver
    {
        public override void Resolve(ResolveParams rp)
        {
            Map map = BaseGen.globalSettings.map;
            CellRect spawnRect = rp.rect.ContractedBy(2);
            int maxSpawn = new IntRange(2, 4).RandomInRange;
            int spawned = 0;
            foreach (IntVec3 current in spawnRect)
            {
                if (spawned >= maxSpawn) break;
                Thing thing = ThingMaker.MakeThing(ThingDefOf.Hay, null);
                thing.stackCount = ThingDefOf.Hay.stackLimit;
                GenSpawn.Spawn(thing, current, map);
                spawned++;
            }
        }
    }
}
