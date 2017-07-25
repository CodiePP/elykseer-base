---
title: Encryption strategy
---

# Stream-oriented

Encryption, and decryption, is tailored towards processing of streams of data.
A memory buffer temporarily holds the file content. The size of this buffer
can be set from 4 MB to 64 MB. 

![filled buffer](../images/hiw01.png)

The buffer is filled with file content row by row.

# Standard AES 256 bit encryption

Once the buffer's capacity is full it is encrypted using a random key (used only once).
Industry standard high-grade encryption using the algorithm AES 256 bit in CBC mode is employed. Today, a very safe algorithm.


# Slicing, non-local data

The columns in the encrypted buffer are randomly shuffeled and
the encrypted buffer is sliced column by column and written to individual files
of 256 KB each.

This has the effect that encrypted data in the extracted files is not local with respect to the original file content. In other words, two adjacent bytes in the original file content are not part of the same encrypted and extracted file.
Thus, to reconstruct the buffer one needs to correctly arrange the columns before decrypting.

The complexity of arranging these columns is detailed [here](../posts/2017-07-19-complexity.html).


# Anonymity

The extracted slices of the encrypted buffer are stored in individual files.
Each file is exactly 256 KB in size and does not have any characteristics which
would allow to determine its column number and thus the reconstruction of the
assembly of slices.


# Private data

The extracted slices from the encrypted buffer can be stored on any filesystem, or even be put into the cloud, to form the data archive.

However, it is recommended to keep a partition of these data in a secure and private place. Anybody wanting to reconstruct the encrypted buffer and starting
to guess the encryption key will always miss some data, thus cannot rebuild the assembly.

This greatly increases the strength of the encryption (see [here](../posts/2017-07-19-complexity.html)).


# Distribution

It is advised to partition the encrypted data into two sets. One is stored on a reliable cloud service or with an existing [backup](../posts/2017-07-19-app-backup.html) solution, the other is kept private and stored inhouse redundantly on different storage systems.


# visual animation

A visual animation of the steps involved in the encryption is
shown [here](../html/anim1.html).

