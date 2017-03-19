using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.AI;
using RimWorld;
using RimWorld.BaseGen;

namespace tribble
{
    class SymbolResolver_ClearForce : SymbolResolver
    {
        private static List<Thing> tmpThingsToDestroy = new List<Thing>();

        public override void Resolve(ResolveParams rp)
        {
            TerrainGrid terrainGrid = BaseGen.globalSettings.map.terrainGrid;
            CellRect.CellRectIterator iterator = rp.rect.GetIterator();
            while (!iterator.Done())
            {
                terrainGrid.RemoveTopLayer(iterator.Current, false);
                //if (rp.clearEdificeOnly.HasValue && rp.clearEdificeOnly.Value)
                //{
                
                    Building edifice = iterator.Current.GetEdifice(BaseGen.globalSettings.map);
                    if (edifice != null && edifice.def.destroyable)
                    {
                        edifice.Destroy(DestroyMode.Vanish);
                    }
                //}
                //else
               // {
                    tmpThingsToDestroy.Clear();
                    tmpThingsToDestroy.AddRange(iterator.Current.GetThingList(BaseGen.globalSettings.map));
                    for (int i = 0; i < tmpThingsToDestroy.Count; i++)
                    {
                        //if (tmpThingsToDestroy[i].def.destroyable || tmpThingsToDestroy[i].def.pathCost > 0)
                        bool allow = Thing.allowDestroyNonDestroyable;
                        
                        Thing.allowDestroyNonDestroyable = true;
                        {
                            tmpThingsToDestroy[i].Destroy(DestroyMode.Vanish);
                            //Log.Message("Destroying " + tmpThingsToDestroy[i]);
                        }
                        Thing.allowDestroyNonDestroyable = allow;
                    }
                //}
                iterator.MoveNext();
            }
        }
    }
}
