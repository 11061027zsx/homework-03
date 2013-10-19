using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maxsum
{
    class AnyMaxsum
    {
        bool[,] gTemp,gResult;
        bool isvselect=false, ishselect=false;
        int sumall=0,n,m,cpuload,amax;
        public int sum=-10000, nsum = 0;

        int[,] a, asum, gmain;
        int[] dx={-1,0,1,0},dy={0,-1,0,1};


        void init()
        {
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    gTemp[i, j] = gResult[i, j] = false;
                    gmain[i, j] = 0;
                }
            
        }

        public AnyMaxsum(int Arrm,int Arrn,int[,] ArrSource)
        {
            n = Arrn;
            m = Arrm;
            a = new int[m, n];
            asum = new int[m, n];

            gTemp = new bool[m, n];
            gmain = new int[m, n];
            gResult = new bool[m, n];
            
            a = ArrSource;
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    if (a[i, j] > 0) sumall += a[i, j];
                    asum[i, j] = sumall;
                    if (amax < a[i, j]) amax = a[i, j];
                }

        }

        public void setvh(bool v,bool h)
        {
            isvselect = v;
            ishselect = h;
        }

        public void GetResultData(ref int s,ref bool[,] aresult)
        {
            init();
            if (sumall <= 0)
            {
                int x = 0, y = 0, z = -1234567891;
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++) if (z < a[i, j])
                        {
                            z = a[i, j];
                            x = i;
                            y = j;
                        }
                s = z;
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++) aresult[i, j] = false;
                aresult[x, y] = true;
                return;
            }

            fany(0, -1, 0);
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++) aresult[i, j] = gResult[i, j];
                    s = sum;
        }
        public int GetCpuLoad()
        {
            return cpuload;
        }

        bool isinvh(int x,int y)
        {
            if (x >= 0 && x < m && y >= 0 && y < n)
                return true;
            return false;
        }

        void ff(int x, int y)
        {
            int tx, ty, i;

            gTemp[x, y] = true;

            for (i = 0; i < 4; i++)
            {
                tx = x + dx[i]; 
                ty = y + dy[i];
                
                if (isvselect)
                {
                    if (tx == m) tx = 0;
                    if (tx == -1) tx = m - 1;
                }

                if (ishselect)
                {
                    if (ty == n) ty = 0;
                    if (ty == -1) ty = n - 1;
                }

                if (isinvh(tx, ty) && !gTemp[tx,ty] && gmain[tx,ty]<2)
                    ff(tx, ty);
            }
        }

        void fany(int x, int y, int z)
        {
            int tx, ty, tt, i, j;

            cpuload++;
            
            for (i = 0; i <= x; i++)
                for (j = 0; j < n; j++)
                    if (gmain[i, j] == 1)
                    {
                        tx = i; ty = j;
                        for (i = 0; i < m; i++)
                            for (j = 0; j < n; j++) gTemp[i, j] = false;
                        ff(tx, ty);
                        for (i = 0; i <= x; i++)
                            for (j = 0; j < n; j++) if (!gTemp[i, j] && gmain[i, j] == 1) return;
                        i = x + 1;
                        break;
                    }


            if (x == m - 1 && y == n - 1)
            {

                
                if (sum > z) return;

                tt = 0;
                for (i = 0; i < m; i++)
                    for (j = 0; j < n; j++) if (gmain[i, j] == 1)
                            tt++;

                if (sum == z && tt > nsum) return;

                for (i = 0; i < m; i++)
                    for (j = 0; j < n; j++) if (gmain[i, j] == 1)
                            gResult[i, j] = true;
                        else gResult[i, j] = false;
                
                sum = z;
                nsum = tt;
                return;
            }

            ty = (y + 1) % n;
            tx = x + (y + 1) / n;

            tt = sumall - asum[tx, ty] + z;

            if (tt + a[tx, ty] >= sum)
            {
                gmain[tx, ty] = 1;
                fany(tx, ty, z + a[tx, ty]);
                gmain[tx, ty] = 0;
            }
            if (tt >= sum)
            {
                gmain[tx, ty] = 2;
                fany(tx, ty, z);
                gmain[tx, ty] = 0;
            }
        }

    }
}
