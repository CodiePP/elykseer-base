---
title: Redundancy
---

# Distribution

We distribute the encrypted data (N slices) over a number (n) of storage sites. They may not be colocated or otherwise linked, so we assume they are independent.

A storage entity has a failure rate of F = 25% per annum.

Redundant copies (r) are always stored on different storage entities.
We store N * r * 256 KB on n disks.

We are hit if r disks fail, with each having a copy of a specific slice.

The probability to fail one replica: r / (n/F)
The probability to fail two replica: r / (n/F)^2^
..
The probability to fail all replica: r / n^r^


# MTTF - Mean time to failure

The average time until a failure is observed.



