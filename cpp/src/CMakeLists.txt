cmake_minimum_required(VERSION 2.8)

# search path for headers
include_directories(".")
include_directories("../../../sharpPRNG_development/src")

# link external libraries
link_directories("../../../../sharpPRNG_development")
set(libs "dl" "z" "prngCpp_s.osx.1.0.4")
#set(libs "z")

# list of source files to be compiled
file(GLOB SRCS *.cpp)

add_library (${PROJECT_NAME}_${CMAKE_BUILD_TYPE} SHARED
	${SRCS}
)

add_library (${PROJECT_NAME}_${CMAKE_BUILD_TYPE}_s STATIC
	${SRCS}
)

if (CMAKE_BUILD_TYPE STREQUAL "DebugProfile")
	add_library (${PROJECT_NAME}_p${CMAKE_BUILD_TYPE} STATIC
		${SRCS}
	)
	set_target_properties(${PROJECT_NAME}_p${CMAKE_BUILD_TYPE} PROPERTIES COMPILE_FLAGS "-fprofile-arcs -ftest-coverage")
	#set(libs, "${libs} gcov")
	target_link_libraries (${PROJECT_NAME}_p${CMAKE_BUILD_TYPE}
		${libs}
		${Boost_LIBRARIES}
	)
endif()


target_link_libraries (${PROJECT_NAME}_${CMAKE_BUILD_TYPE} PUBLIC
	${libs}
	${Boost_LIBRARIES}
)

target_link_libraries (${PROJECT_NAME}_${CMAKE_BUILD_TYPE}_s
	${libs}
	${Boost_LIBRARIES}
)

