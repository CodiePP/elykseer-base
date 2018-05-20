cmake_minimum_required(VERSION 3.5)

#set(CMAKE_MODULE_PATH ${CMAKE_MODULE_PATH} "${CMAKE_SOURCE_DIR}/cmake/Modules/")

IF(DEFINED CMAKE_BUILD_TYPE)
	SET(CMAKE_BUILD_TYPE ${CMAKE_BUILD_TYPE} CACHE STRING "Choose the type of build, options are: None(CMAKE_CXX_FLAGS or CMAKE_C_FLAGS used) Debug Release DebugProfile RelWithDebInfo MinSizeRel.")
ELSE()
	SET(CMAKE_BUILD_TYPE Debug CACHE STRING "Choose the type of build, options are: None(CMAKE_CXX_FLAGS or CMAKE_C_FLAGS used) Debug Release DebugProfile RelWithDebInfo MinSizeRel.")
ENDIF()

project(elykseer-base)

# The version number.
set (VERSION_MAJOR 1)
set (VERSION_MINOR 0)

# choose either one
set(USE_OPENSSL NO)
set(USE_CRYPTOPP YES)

#set(CMAKE_C_COMPILER "/usr/local/bin/gcc48")
#set(CMAKE_CXX_COMPILER "/usr/local/bin/g++48")
#set(CMAKE_CXX_COMPILER "/usr/local/bin/clang36")

#add_definitions( -DLinux )
#add_definitions( -DFreeBSD )
#add_definitions( -DWindows )


# boost
set(BOOST_ROOT "${CMAKE_HOME_DIRECTORY}/ext/boost_1_67_0/")
set(BOOST_LIBRARYDIR "${CMAKE_HOME_DIRECTORY}/ext/boost_1_67_0/stage/lib")
add_definitions( -DBOOST_ALL_DYN_LINK )
#set(Boost_ADDITIONAL_VERSIONS "1.49")
set(Boost_DEBUG OFF)
set(Boost_USE_STATIC_LIBS OFF)
set(Boost_USE_STATIC_RUNTIME OFF)
set(Boost_USE_MULTITHREADED ON)
find_package( Boost 1.67.0 REQUIRED COMPONENTS system filesystem chrono thread date_time iostreams contract )
LINK_DIRECTORIES ( ${Boost_LIBRARY_DIRS} )
INCLUDE_DIRECTORIES (${Boost_INCLUDE_DIRS})

# cryptography
IF(USE_CRYPTOPP)
  add_definitions( -DCRYPTOPP )
  add_definitions( -DCRYPTOLIB=CRYPTOPP )
  add_library(libcryptopp STATIC IMPORTED)
  SET_TARGET_PROPERTIES(libcryptopp PROPERTIES IMPORTED_LOCATION ${CMAKE_HOME_DIRECTORY}/ext/cryptopp/libcryptopp.a)
  set(CRYPTO_LIBS "libcryptopp")
ENDIF(USE_CRYPTOPP)

IF(USE_OPENSSL)
  add_definitions( -DOPENSSL )
  add_definitions( -DCRYPTOLIB=OPENSSL )
  IF(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
    set(OPENSSL_ROOT_DIR "/usr/local/opt/openssl")
  ENDIF(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
  find_package(OpenSSL REQUIRED)
  include_directories(${OPENSSL_INCLUDE_DIR})
  set(CRYPTO_LIBS ${OPENSSL_CRYPTO_LIBRARY} ${OPENSSL_SSL_LIBRARY})
ENDIF(USE_OPENSSL)


#set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11 -fopenmp -g -O1")
# clang also wants its own stdlib
#set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11 -stdlib=libc++ -g -O0")
#set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++17 -g -O0 -fPIC")

set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++1z -fPIC -g")
set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -std=c99 -fPIC -g")

set(CMAKE_LD_FLAGS "${CMAKE_LD_FLAGS} -fPIC -g")

# OS dependent
IF(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")
  set(CMAKE_OSX_DEPLOYMENT_TARGET "10.11.0")
ENDIF(${CMAKE_SYSTEM_NAME} MATCHES "Darwin")

# other external tools
include_directories(./ext)


add_subdirectory( cpp )

