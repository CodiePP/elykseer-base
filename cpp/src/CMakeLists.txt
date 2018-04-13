cmake_minimum_required(VERSION 2.8)

# search path for headers
include_directories("..")
include_directories("${CMAKE_HOME_DIRECTORY}/../sharpPRNG_development/src")
include_directories("${CMAKE_HOME_DIRECTORY}/ext/libb64/include")
include_directories("${CMAKE_HOME_DIRECTORY}/ext/date/include")
include_directories("${CMAKE_HOME_DIRECTORY}/ext/pugixml/src")

add_library(libb64 STATIC IMPORTED)
SET_TARGET_PROPERTIES(libb64 PROPERTIES IMPORTED_LOCATION ${CMAKE_HOME_DIRECTORY}/ext/libb64/src/libb64.a)

# link external libraries
link_directories("../../../../sharpPRNG_development")
IF(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
   # Mac OS X specific code
   set(libs "dl" "z" "prngCpp_s.osx.1.0.4")
ENDIF(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
IF(${CMAKE_SYSTEM_NAME} MATCHES "Linux")
  # Linux specific code
  set(libs "dl" "z" "prngCpp_s.linux.1.0.4")
ENDIF(${CMAKE_SYSTEM_NAME} MATCHES "Linux")

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
    ${CRYPTO_LIBS}
	)
endif()


target_link_libraries (${PROJECT_NAME}_${CMAKE_BUILD_TYPE} PUBLIC
	${libs}
	${Boost_LIBRARIES}
  ${CRYPTO_LIBS}
  libb64
)

target_link_libraries (${PROJECT_NAME}_${CMAKE_BUILD_TYPE}_s
	${libs}
	${Boost_LIBRARIES}
  ${CRYPTO_LIBS}
  libb64
)

