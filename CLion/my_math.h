//
// Created by net_s on 2019/07/05.
//
#pragma once

#ifndef CLION_MY_MATH_H
#define CLION_MY_MATH_H
#endif //CLION_MY_MATH_H


#define __HI(x) *(1+(int*)&x)
#define __LO(x) *(int*)&x


double ph_sin(double x);
double ph_cos(double x);
double ph_tan(double x);

int __ieee754_rem_pio2(double x, double *y);

double scalbn (double x, int n);
double copysign(double x, double y);
double floor(double x);

double __kernel_cos(double x, double y);
double __kernel_sin(double x, double y, int iy);
double __kernel_tan(double x, double y, int iy);
int __kernel_rem_pio2(double *x, double *y, int e0, int nx, int prec, const int *ipio2);


double fabs(double x);
