#!/bin/sh

if [ ! -d ml ]; then
  mkdir -v ml
fi
if [ ! -d fs ]; then
  mkdir -v fs
fi

FST=$(ls lxr.*.fst)

for F in $FST; do

  echo $F
  fstar.exe --record_hints --use_hints --cache_checked_modules --timing $F
  fstar.exe --odir ml --codegen OCaml $F
  fstar.exe --odir fs --codegen FSharp $F

done

