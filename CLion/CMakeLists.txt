cmake_minimum_required(VERSION 3.10)
project(CLion C)

set(CMAKE_C_STANDARD 11)

add_executable(CLion main.c my_math.h s_sin.c s_cos.c s_tan.c e_rem_pio2.c k_rem_pio2.c s_scalbn.c s_copysign.c s_floor.c k_cos.c k_sin.c k_tan.c s_fabs.c input_parameters.c input_parameters.h)

target_link_libraries(CLion m)