locals {
  lambda_artifact_source = "../publish/DivvyExercise.Lambda.zip"
  lambda_artifact_md5    = "${filemd5(local.lambda_artifact_source)}"
}

resource "aws_s3_bucket_object" "lambda_artifact" {
  key    = "${local.lambda_artifact_md5}.zip"
  bucket = "${aws_s3_bucket.lambda.id}"
  source = "${local.lambda_artifact_source}"
  etag   = "${local.lambda_artifact_md5}"
  tags   = "${local.common_tags}"
}
