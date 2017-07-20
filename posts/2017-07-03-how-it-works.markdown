---
title: How it works.
---

# 1)

We create a buffer to hold the data temporarily. Then, fill in the data to be archived.

![](../images/hiw01.png)


# 2)

The buffer is encrypted using AES in [chain-block mode](https://en.wikipedia.org/wiki/Block_cipher_mode_of_operation#CBC).

![](../images/hiw02.png)


# 3)

Columns are shuffeled randomly.

![](../images/hiw03.png)


# 4)

The buffer is rotated such that we can extract data from columns.
The data in the column does not show the same locality as in a row.

![](../images/hiw04.png)


# 5)

The encrypted data in columns are saved to individual files.
These files can be stored in different locations and on various online cloud services.

![](../images/hiw05.png)


# 6)

We keep the information of how to arrange the raw data so to reconstruct the original buffer.
This is an extra key in addition to the AES key, which makes the encryption more effective.

![](../images/hiw06.png)


# animation

We have added an [animation](../html/anim1.html) which details the steps of encryption and extraction.

