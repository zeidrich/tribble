using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    class SymbolResolver_FillWithCells : SymbolResolver
    {

        private void addCell(ResolveParams rp, ref List<int> cellList, string symbol, bool clear = true)
        {
            if (cellList.Count > 0)
            { 
                int firstPick = new IntRange(0, cellList.Count-1).RandomInRange;
                //Log.Message("pick is " + firstPick);
                //Log.Message(cellList.ToString());
                int farm1 = cellList[firstPick];
                //Log.Message("value is " + farm1);
                cellList.RemoveAt(firstPick);
                ResolveParams rpFarm1 = rp;
                int farm1x = farm1 % 3;
                int farm1z = (farm1 - farm1x) / 3;

                farm1x = rp.rect.minX + 2 + farm1x * 12;
                farm1z = rp.rect.minZ + 2 + farm1z * 12;

                rpFarm1.rect.minX = farm1x;
                rpFarm1.rect.maxX = rpFarm1.rect.minX + 10;
                rpFarm1.rect.minZ = farm1z;
                rpFarm1.rect.maxZ = rpFarm1.rect.minZ + 10;

                rpFarm1.rect = rpFarm1.rect.ContractedBy(1);
            
                BaseGen.symbolStack.Push(symbol, rpFarm1);
                BaseGen.symbolStack.Push("clearForce", rpFarm1);
            }
        }

        public override void Resolve(ResolveParams rp)
        {
            Map map = BaseGen.globalSettings.map;
            bool @bool = Rand.Bool;
            ThingDef thingDef = ThingDefOf.TorchLamp;
            Lord singlePawnLord = rp.singlePawnLord ?? LordMaker.MakeNewLord(rp.faction, new LordJob_DefendBase(rp.faction, rp.rect.CenterCell), map, null);
            rp.singlePawnLord = singlePawnLord;
            List<int> cellList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            addCell(rp, ref cellList, "farm");
            addCell(rp, ref cellList, "farm");
            while (cellList.Count > 0)
            {
                Log.Message("adding room");
                addCell(rp, ref cellList, "bedroom");
            }
            /*
            foreach (IntVec3 current in rp.rect)
            {
                if ((current.x - rp.rect.minX + 12) % 13 != 0 || (current.z - rp.rect.minZ + 12) % 13 != 0)
                {
                    continue;
                }
                ResolveParams rp2 = rp;
                rp2.rect.minX = current.x + 1;
                rp2.rect.minZ = current.z + 1;
                rp2.rect.maxX = rp2.rect.minX + 9;
                rp2.rect.maxZ = rp2.rect.minZ + 9;
                if (rp2.rect.maxX < rp.rect.maxX && rp2.rect.maxZ < rp.rect.maxZ)
                {
                    rp2.singlePawnLord = resolveParams.singlePawnLord;
                    rp2.singlePawnKindDef = (rp2.singlePawnKindDef ?? PawnKindDef.Named("Pirate"));
                    rp2.singlePawnKindDef.combatPower = 250f;
                    //rp2.singlePawnKindDef.combatPower = 2500f;
                    //rp2.singlePawnKindDef.weaponTags.Clear();
                    //rp2.singlePawnKindDef.weaponTags.Add("Gun");
                    //rp2.singlePawnKindDef.weaponTags.Add("Melee");
                    //rp2.singlePawnKindDef.weaponMoney = new FloatRange(10f, 1000f);
                    //rp2.singlePawnKindDef.fixedInventory.Add(new ThingCountClass(ThingDefOf.Gun_Pistol, 1));

                    //BaseGen.symbolStack.Push("pawn", rp2);
                    //BaseGen.symbolStack.Push("doors", rp2);
                    
                    BaseGen.symbolStack.Push("farm", rp2);
                    BaseGen.symbolStack.Push("clearForce", rp2);
                    //BaseGen.symbolStack.Push("unflooredEmpty", rp2);
                }
            }
            */
        }
    }
}
