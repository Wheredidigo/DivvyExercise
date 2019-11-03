resource "aws_sns_topic_subscription" "data_ingest_notifications_subscription" {
  topic_arn            = "${aws_sns_topic.data_ingest_notifications.arn}"
  protocol             = "sqs"
  endpoint             = "${aws_sqs_queue.data_file_events.arn}"
  raw_message_delivery = true
}
