resource "aws_s3_bucket_policy" "data_ingest_bucketpolicy" {
  bucket = "${aws_s3_bucket.data_ingest_bucket.id}"

  policy = <<POLICY
{
  "Id": "l0 Bucket Policy",
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "Allow Lambda to Get objects",
      "Action": [
        "s3:GetObject"
      ],
      "Effect": "Allow",
      "Resource": "${aws_s3_bucket.data_ingest_bucket.arn}/*",
      "Principal": {
        "AWS": ["${aws_iam_role.lambda_role.arn}"]
      }
    },
    {
      "Sid": "Allow Lambda to see the Bucket",
      "Action": [
        "s3:GetBucketLocation",
        "s3:ListBucket"
      ],
      "Effect": "Allow",
      "Resource": "${aws_s3_bucket.data_ingest_bucket.arn}",
      "Principal": {
        "AWS": ["${aws_iam_role.lambda_role.arn}"]
      }
    }
  ]
}
POLICY
}
