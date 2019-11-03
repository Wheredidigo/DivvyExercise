resource "aws_s3_bucket_notification" "data_ingest_notification" {
  bucket = "${aws_s3_bucket.data_ingest_bucket.id}"

  topic {
    topic_arn     = "${aws_sns_topic.data_ingest_notifications.arn}"
    events        = ["s3:ObjectCreated:*"]
  }
}