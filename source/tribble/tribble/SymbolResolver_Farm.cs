using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    class SymbolResolver_Farm : SymbolResolver
    {
        
        public override void Resolve(ResolveParams rp)
        {
            Map map = BaseGen.globalSettings.map;
            float viability = GrowableTiles(rp.rect, map);
            if (viability > 0.5f && map.mapTemperature.OutdoorTemp >= 5f)
            {
                CreateGarden(rp);   
            }
            else
            {
                CreatePen(rp);
            }
            
        }

        private void CreateGarden(ResolveParams rp)
        {
            ThingDef plantDef;
            Plant result;
            Map map = BaseGen.globalSettings.map;
            foreach (IntVec3 current in rp.rect)
            {
                plantDef = ThingDef.Named("PlantRice");
                if (CalculateFertilityAt(current, map) < 1.0f)
                {
                    plantDef = ThingDef.Named("PlantPotato");
                }
                if (plantDef.CanEverPlantAt(current, map))
                {
                    result = (Plant)GenSpawn.Spawn(plantDef, current, map);
                    result.Growth = new FloatRange(0.5f, 0.85f).RandomInRange;

                }
            }
        }
        private void CreatePen(ResolveParams rp)
        {
            Map map = BaseGen.globalSettings.map;
            ResolveParams penRp = rp;
            penRp.rect.maxZ = (penRp.rect.maxZ + penRp.rect.minZ) / 2;
            Faction faction = rp.faction;
            Lord singlePawnLord = LordMaker.MakeNewLord(faction, new LordJob_StayInPen(penRp.faction, penRp.rect.CenterCell), map, null);
            ResolveParams doorRp = penRp;
            ResolveParams outsideRp = rp;
            outsideRp.rect.minZ = penRp.rect.maxZ;
            doorRp.rect.minX = ((doorRp.rect.maxX + doorRp.rect.minX) / 2) - 1;
            doorRp.rect.minZ = doorRp.rect.maxZ;
            doorRp.rect.maxX = doorRp.rect.minX + 2;
            ResolveParams outsideDoorRp = outsideRp;
            outsideDoorRp.rect.minZ = outsideDoorRp.rect.maxZ;
            outsideDoorRp.rect.minX = ((outsideDoorRp.rect.maxX + outsideDoorRp.rect.minX) / 2) - 1;
            outsideDoorRp.rect.maxX = outsideDoorRp.rect.minX + 2;
            outsideDoorRp.rect.minZ = outsideDoorRp.rect.maxZ;
            ResolveParams chickenRp = penRp;
            chickenRp.singlePawnLord = singlePawnLord;
            chickenRp.singlePawnKindDef = (PawnKindDef.Named("Chicken"));
            chickenRp.rect = chickenRp.rect.ContractedBy(1);
            //chickenRp.faction = null;
            int numChickens = new IntRange(6, 10).RandomInRange;
            for (int i = 0; i < numChickens; i++)
            {
                BaseGen.symbolStack.Push("pawn", chickenRp);
            }
            BaseGen.symbolStack.Push("heatingCooling", chickenRp);
            BaseGen.symbolStack.Push("hay", outsideRp);
            BaseGen.symbolStack.Push("doors", doorRp);
            BaseGen.symbolStack.Push("doors", outsideDoorRp);
            BaseGen.symbolStack.Push("unflooredEmpty", outsideRp);
            BaseGen.symbolStack.Push("roof", penRp);
            BaseGen.symbolStack.Push("edgeWalls", penRp);
            BaseGen.symbolStack.Push("floor", penRp);
        }


        private float GrowableTiles(CellRect rect, Map map)
        {
            float returnGrowableTiles = 0;
            float numTiles = 0;
            float numGrowableTiles = 0;
            foreach (IntVec3 current in rect)
            {
                numTiles += 1f;
                if (ThingDefOf.PlantPotato.CanEverPlantAt(current, map))
                {
                    numGrowableTiles += 1f;
                }
            }
            if (numTiles > 0f)
            {
                returnGrowableTiles = numGrowableTiles / numTiles;
            }
            return returnGrowableTiles;
        }

        private float CalculateFertilityAt(IntVec3 loc, Map map)
        {
            Thing edifice = loc.GetEdifice(map);
            if (edifice != null && edifice.def.fertility >= 0f)
            {
                //Log.Message("Non-null edifice calculating fertility " + edifice.def);
                return edifice.def.fertility;
            }
            return map.terrainGrid.TerrainAt(loc).fertility;
        }
    }
}
