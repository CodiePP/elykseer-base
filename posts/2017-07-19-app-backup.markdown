---
title: Application Backup
---

# Extend storage capacity

LXR can be used to extend storage capacity of an existing backup solution.
Not all data needs the same level of security when it comes to its long-term storage.

Using LXR's sophisticated encryption algorithm, we achieve a high ratio of
data that can be put on COTS (commercial off-the-shelf) hardware or even be 
uploaded to the cloud, vs. data that is of high value 
(i.e. encryption keys, meta data) that need a firm security perimeter to be
stored securely.

# Compression

Data encrypted with LXR is so safe that it can be uploaded to the cloud.
The generated encryption keys and the information describing the original files must however be kept private. 

We estimate the ratio of sensitive, private information vs. encrypted data:

> **1 : 100**

In practice, one should keep some encrypted data in private (e.g. 10%),
so this ratio comes down to about 1:10.

# Deduplication

LXR includes a versatile deduplication algorithm at the file or at the block level.

Deduplication at the file level works like this:
if a file does not change from one backup to another, its content does not need to be encrypted again.

Deduplication at the block level looks at blocks (e.g. 64 KB) of a file and compares these to previously encrypted blocks of that file. If some part of a file changes from one backup to another, then the corresponding block is encrypted anew.

# Save money

Encrypted data can be stored cost-efficiently on COTS hardware. We prefer to setup distributed filesystems on existing harware (bare PCs) and set a high replication factor (i.e. 6x). Open-Source solutions exist and we have experience with: [hdfs (Hadoop)](https://hadoop.apache.org), [xtreemfs](http://www.xtreemfs.org), [moosefs](https://moosefs.com).

No need to buy new backup capacity: use the existing solution to store private data (i.e. encryption keys, meta data, part of encrypted data).
This leads to **massive cost savings**.

