# Opencv classifier readme

Files are named as follows: cascade\_[number of levels]\_[intended use]\_[feature type].xml

In general, the greater the number of levels, the more selective the classifier is (fewer false positives but also maybe misses more).

The two feature types are lbp and haar. Haar takes longer to train and is supposed to perform slightly better. In practice there is no obvious difference in performance.

To change which classifier is used, change the classifierPath in KinectClassify. 