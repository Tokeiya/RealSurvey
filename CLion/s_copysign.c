//
// Created by net_s on 2019/07/05.
//

#include "my_math.h"

#ifdef __STDC__
double copysign(double x, double y)
#else
double copysign(x,y)
	double x,y;
#endif
{
    __HI(x) = (__HI(x)&0x7fffffff)|(__HI(y)&0x80000000);
    return x;
}

