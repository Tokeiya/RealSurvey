#include <stdio.h>
#include <math.h>
#include "my_math.h"
#include "input_parameters.h"


static const char* compiler_type=
#ifdef __GNUC__
#ifndef __clang__
        "GCC.tsv";
#else
        "clang.tsv";
#endif
#else
        "NA.tsv";
#endif

static const int session_id=
#ifdef __GNUC__
#ifndef __clang__
        42;
#else
        23;
#endif
#else
114514;
#endif



int main()
{
    printf(compiler_type);

    FILE *fp=fopen(compiler_type,"w");

    fprintf(fp,"SessionId\tInputDegrees\tInputRadians\tRadians\tHighPrecisionRadians\tSin\tCos\tTan\tSin/Cos\tPayneHanekSin\tPayneHanekCos\tPayneHanekTan\n");

    for (int i = 0; i < parameters_size; ++i) {
        input_parameter piv= parameters[i];

        double std_sin=sin(piv.input_radians);
        double std_cos=cos(piv.input_radians);
        double std_tan=tan(piv.input_radians);
        double sin_cos=std_sin/std_cos;

        double p_sin=ph_sin(piv.input_radians);
        double p_cos=ph_cos(piv.input_radians);
        double p_tan=ph_tan(piv.input_radians);

        //SessionId	InputDegrees	InputRadians	Radians	HighPrecisionRadians	Sin	Cos	Tan	Sin/Cos	PayneHanekSin	PayneHanekCos	PayneHanekTan
        fprintf(fp,"%d\t%.17g\t%.17g\t%.17g\t%.17g\t%.17g\t%.17g\t%.17g\t%.17g\t%.17g\t%.17g\t%.17g\n",session_id,piv.input_degrees,piv.input_radians,piv.input_radians,piv.input_radians,std_sin,std_cos,std_tan,sin_cos,p_sin,p_cos,p_tan);


    }

    fclose(fp);


}


void hoge() {

    FILE *fp=fopen("output.txt","w");

    for (int i = 9000; i >= -9000; --i)
    {
        //double rad=(i*3.14)/180.0;
        //double s= ph_sin(rad);
        //double c=cos(rad);
        //double t=tan(rad);

        //fprintf(fp,"%.17g\t%.17g\t%.17g\t%.17g\n",rad,s,c,t);

    }

    fclose(fp);

}