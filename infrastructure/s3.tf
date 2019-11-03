resource "aws_s3_bucket" "data_ingest_bucket" {

  bucket = "${local.account_id}-data-ingest-${local.region}"
  acl    = "private"
  region = "${local.region}"

  server_side_encryption_configuration {
    rule {
      apply_server_side_encryption_by_default {
        sse_algorithm = "AES256"
      }
    }
  }

  versioning {
    enabled = true
  }

  lifecycle_rule {
    id      = "remove_objects"
    enabled = true

    expiration {
      days = 7
    }

    noncurrent_version_expiration {
      days = 1
    }
  }

  tags = "${local.common_tags}"
}

resource "aws_s3_bucket" "lambda" {
  bucket = "${local.account_id}-lambda-processor-${local.region}"
  acl    = "private"
  region = "${local.region}"

  server_side_encryption_configuration {
    rule {
      apply_server_side_encryption_by_default {
        sse_algorithm = "AES256"
      }
    }
  }

  versioning {
    enabled = true
  }

  lifecycle_rule {
    id      = "remove_old_versions"
    enabled = true

    noncurrent_version_expiration {
      days = 7
    }
  }

  tags = "${local.common_tags}"
}
