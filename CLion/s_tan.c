//
// Created by net_s on 2019/07/05.
//
#include "my_math.h"

#ifdef __STDC__
double ph_tan(double x)
#else
double tan(x)
	double x;
#endif
{
    double y[2],z=0.0;
    int n, ix;

    /* High word of x. */
    ix = __HI(x);

    /* |x| ~< pi/4 */
    ix &= 0x7fffffff;
    if(ix <= 0x3fe921fb) return __kernel_tan(x,z,1);

        /* tan(Inf or NaN) is NaN */
    else if (ix>=0x7ff00000) return x-x;		/* NaN */

        /* argument reduction needed */
    else {
        n = __ieee754_rem_pio2(x,y);
        return __kernel_tan(y[0],y[1],1-((n&1)<<1)); /*   1 -- n even
							-1 -- n odd */
    }
}
