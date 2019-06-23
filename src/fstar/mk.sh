#!/bin/sh

FSTAR="fstar.exe"

if [ ! -d ml ]; then
  mkdir -v ml
fi
if [ ! -d fs ]; then
  mkdir -v fs
fi

FST=$(ls lxr.*.fst)

for F in $FST; do

  echo $F
  ${FSTAR} --record_hints --use_hints --cache_checked_modules --timing $F
  ${FSTAR} --odir ml --codegen OCaml $F
  ${FSTAR} --odir fs --codegen FSharp $F

done

