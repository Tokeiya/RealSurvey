//
// Created by net_s on 2019/07/05.
//

#include "my_math.h"


#ifdef __STDC__
double fabs(double x)
#else
double fabs(x)
	double x;
#endif
{
    __HI(x) &= 0x7fffffff;
    return x;
}

