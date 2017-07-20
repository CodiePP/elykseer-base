---
title: Backup overview.
---

![](../images/Operations_Backup.png)

# encryption

We employ standard AES encryption with 256 bit key lenght in CBC mode (block chaining).
A random key is generated for every block of 64 MB (can be adjusted). 

# deduplication

If we have previously made a copy of a file, we have a record of the file's meta data
and can either skip encryption if the content of the file has not changed, or only 
backup the parts that are modified or added to the file.

# operations

The processing of data through encryption and extraction generates three types of information
which need to be stored in seperate places.

### encryption keys

These keys are needed if the file is to be restored from the extracted data at some point in the future. This information must be kept in a safe place. If ever these keys were lost, the original data cannot be restored anymore from the encrypted and extracted files.

### file meta data

This information records a number of information about the files which were encrypted using LXR.
Using this we can relate an archived file with the encrypted data in the long-term storage.

### encrypted data

In the archive we only keep strongly encrypted data. It is now safe to make copies of these data and put it on different storage media (disk, DVD) or copy it to a cloud storage service like AWS, Google or Azure.


