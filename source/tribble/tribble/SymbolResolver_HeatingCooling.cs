using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    public class SymbolResolver_HeatingCooling : SymbolResolver
    {
        private const float SpawnCampfireIfTemperatureBelow = -20f;

        private const float SpawnSecondCampfireIfTemperatureBelow = -45f;

        private const float SpawnPassiveCoolerIfTemperatureAbove = 22f;

        private List<int> tmpTakenCorners = new List<int>();

        public override void Resolve(ResolveParams rp)
        {
            Log.Message("Resolving Heating Cooling for " + rp.rect.minX + "," + rp.rect.minZ + " - " + rp.rect.maxX + "," + rp.rect.maxZ);
            this.tmpTakenCorners.Clear();
            Map map = BaseGen.globalSettings.map;
            int coolingCorner;
            if (map.mapTemperature.OutdoorTemp > 22f && BaseGenUtility.TryFindRandomNonDoorBlockingCorner(rp.rect, BaseGen.globalSettings.map, out coolingCorner, this.tmpTakenCorners))
            {
                this.tmpTakenCorners.Add(coolingCorner);
                ResolveParams resolveParams = rp;
                resolveParams.singleThingDef = ThingDefOf.PassiveCooler;
                resolveParams.rect = CellRect.SingleCell(BaseGenUtility.GetCornerPos(rp.rect, coolingCorner));
                Log.Message("adding cooling at " + resolveParams.rect.minX +"," + resolveParams.rect.minZ);
                BaseGen.symbolStack.Push("thing", resolveParams);
            }
            ThingDef singleThingDef = (map.mapTemperature.OutdoorTemp >= -20f) ? ThingDefOf.TorchLamp : ThingDefOf.Campfire;
            int heatersNeeded = (map.mapTemperature.OutdoorTemp >= -45f) ? 1 : 2;
            for (int i = 0; i < heatersNeeded; i++)
            {
                int heatingCorner;
                if (BaseGenUtility.TryFindRandomNonDoorBlockingCorner(rp.rect, BaseGen.globalSettings.map, out heatingCorner, this.tmpTakenCorners))
                {
                    this.tmpTakenCorners.Add(heatingCorner);
                    ResolveParams resolveParams2 = rp;
                    resolveParams2.singleThingDef = singleThingDef;
                    resolveParams2.rect = CellRect.SingleCell(BaseGenUtility.GetCornerPos(rp.rect, heatingCorner));
                    Log.Message("adding heating at " + resolveParams2.rect.minX + "," + resolveParams2.rect.minZ);
                    BaseGen.symbolStack.Push("thing", resolveParams2);
                }
            }
        }
    }
}
