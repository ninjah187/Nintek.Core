using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core
{
    public delegate Task<T> LazyAsync<T>();
    public delegate Task<T> LazyAsync<Param, T>(Param param);
    public delegate Task<T> LazyAsync<P1, P2, T>(P1 p1, P2 p2);
    public delegate Task<T> LazyAsync<P1, P2, P3, T>(P1 p1, P2 p2, P3 p3);
    public delegate Task<T> LazyAsync<P1, P2, P3, P4, T>(P1 p1, P2 p2, P3 p3, P4 p4);
    public delegate Task<T> LazyAsync<P1, P2, P3, P4, P5, T>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    public delegate Task<T> LazyAsync<P1, P2, P3, P4, P5, P6, T>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6);
    public delegate Task<T> LazyAsync<P1, P2, P3, P4, P5, P6, P7, T>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7);
    public delegate Task<T> LazyAsync<P1, P2, P3, P4, P5, P6, P7, P8, T>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8);
}
