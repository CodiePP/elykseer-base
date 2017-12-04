#!/bin/bash

GITALK=`pwd`/../ext/gitalk

cd src

for HPP in `bash $GITALK/utils/find_hpp.sh ../../src/elykseer-base.md`; do
  bash $GITALK/utils/make_hpp.sh ${HPP}
  bash $GITALK/utils/make_cpp.sh ${HPP}
done

cd ..
cd tests

$GITALK/utils/make_test.sh ../../tests/elykseer-base-ut.md

cd ..
