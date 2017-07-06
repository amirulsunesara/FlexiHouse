using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Shelves
    {
        public int id { get; set; }
        public string shelfItems { get; set; }
        public int? slotsRemaining { get; set; }
        public string shelveID { get; set; }
        public int section_id { get; set; }
     
        public double centerPointX { get; set; }
        public double centerPointY { get; set; }
        public int warehouse_id { get; set; }
        public double distance { get; set; }
     
        public char zone { get; set; }


        public int ZoneAshelf(int Shelfcount)
        {
            double a = Math.Round(0.1 * Shelfcount);
            return (int)a;
        }
        public int ZoneBshelf(int Shelfcount)
        {
            double b = Math.Round(0.2 * Shelfcount);
            return (int)b;
        }
        public int ZoneCshelf(int Shelfcount,int zoneAshelf, int zoneBshelf)
        {
            double c = Shelfcount-zoneAshelf-zoneBshelf;
            return (int)c;
        }
        public List<Shelves> ShelvesWithDistance(List<Shelves> shelves, List<Depot> depot)
        {
            Depot dep = depot.ElementAt(0);
            double x2 = dep.centerPointX;
            double y2 = dep.centerPointY;
            int i = 0;
            foreach(Shelves shel in shelves)
            {

                double x1 = shel.centerPointX;
                double y1 = shel.centerPointY;
                shelves.ElementAt(i).distance =Math.Sqrt(((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));

                i++;
            }

            return shelves;
        }
        public List<Shelves> SortShelves(List<Shelves> shelves)
        {
            Shelves temp=new Shelves();
            for (int p = 0; p <= shelves.Count- 2; p++)
            {
                for (int i = 0; i <= shelves.Count - 2; i++)
                {
                    if (shelves.ElementAt(i).distance > shelves.ElementAt(i + 1).distance)
                    {
                        temp = shelves[i + 1];
                        shelves[i + 1] = shelves[i];
                        shelves[i] = temp;
                    }
                }
            }
                return shelves;
        }

        public List<Shelves> assignZones(List<Shelves> shelves , int ZoneA,int ZoneB,int ZoneC) {
            bool isCheck = true;
           bool continuity = false;
            int i = 0;
            foreach (Shelves shelve in shelves)
            { 
                  if(continuity == true)
                {

                    continuity = false;
                    continue;
                }
            

          if (shelves.Count < 5 && isCheck==true)
                {
                    shelves.ElementAt(i).zone = 'A';
                    shelves.ElementAt(i + 1).zone = 'B';
                    ZoneB--;
                    ZoneA--;
                    i=i+2;
                    isCheck = false;
                    continuity = true;
                    continue;
                }
               else if(ZoneA > 0)
                {
                    shelves.ElementAt(i).zone = 'A';
                    ZoneA--;
                    i++;
                    continue;
                }
                else if(ZoneB > 0)
                {
                    shelves.ElementAt(i).zone = 'B';
                    ZoneB--;
                    i++;
                    continue;

                }
                else if(ZoneC > 0)
                {
                    try
                    {
                        shelves.ElementAt(i).zone = 'C';
                        ZoneC--;
                        i++;
                    }
                    catch(Exception ex)
                    {

                    }
                    continue;


                }


            }

            return shelves;
        }
    }
}
