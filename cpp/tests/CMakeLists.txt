cmake_minimum_required(VERSION 2.8)

#project(UT_elykseer-base)

IF(DEFINED CMAKE_BUILD_TYPE)
   SET(CMAKE_BUILD_TYPE ${CMAKE_BUILD_TYPE} CACHE STRING "Choose the type of build, options are: None(CMAKE_CXX_FLAGS or CMAKE_C_FLAGS used) Debug Release DebugProfile RelWithDebInfo MinSizeRel.")
ELSE()
   SET(CMAKE_BUILD_TYPE Debug CACHE STRING "Choose the type of build, options are: None(CMAKE_CXX_FLAGS or CMAKE_C_FLAGS used) Debug Release DebugProfile RelWithDebInfo MinSizeRel.")
ENDIF()

# boost
#set(Boost_ADDITIONAL_VERSIONS "1.49")
set(Boost_DEBUG OFF)
set(Boost_USE_STATIC_LIBS OFF)
set(Boost_USE_STATIC_RUNTIME OFF)
set(Boost_USE_MULTITHREADED ON)
find_package( Boost 1.48.0 REQUIRED COMPONENTS system thread date_time chrono unit_test_framework)
include_directories(${Boost_INCLUDE_DIRS})

# mine
include_directories(../src)
include_directories("../../../sharpPRNG_development/src")

# link external libraries
link_directories("../../../../sharpPRNG_development")
#set(libs "dl" "z" "prngCpp.osx.1.0.4")
set(libs "dl" "z")

file(GLOB SRCS ut*.cpp mocks/*.cpp)

if (CMAKE_BUILD_TYPE STREQUAL "Debug")

        add_executable ( ut${PROJECT_NAME}
          ${SRCS}
        )

        target_link_libraries ( ut${PROJECT_NAME}
          elykseer-base_${CMAKE_BUILD_TYPE}
          ${libs}
          ${Boost_LIBRARIES}
          ${OPENSSL_CRYPTO_LIBRARY}
          ${OPENSSL_SSL_LIBRARY}
        )
endif()

if (CMAKE_BUILD_TYPE STREQUAL "DebugProfile")
        add_executable ( utp${PROJECT_NAME}
                ${SRCS}
        )
        set_target_properties(utp${PROJECT_NAME} PROPERTIES LINK_FLAGS "-fprofile-arcs -ftest-coverage ")
        target_link_libraries ( utp${PROJECT_NAME}
                elykseer-base_p${CMAKE_BUILD_TYPE}
                #gcov
                ${libs}
                ${Boost_LIBRARIES}
                ${OPENSSL_CRYPTO_LIBRARY}
                ${OPENSSL_SSL_LIBRARY}
        )

endif()

