import {
  CloudinaryModule,
  CloudinaryConfiguration
} from '@cloudinary/angular-5.x';

import { Cloudinary } from 'cloudinary-core';

export const SoproplCloudinaryModule = CloudinaryModule.forRoot(
  { Cloudinary },
  {
    cloud_name: 'abdelhadyit'
  } as CloudinaryConfiguration
);
