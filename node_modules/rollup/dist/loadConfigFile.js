/*
  @license
	Rollup.js v2.74.0
	Thu, 19 May 2022 05:01:43 GMT - commit fc99e96e09d26798f1c0aabdd7429307cc908c8e

	https://github.com/rollup/rollup

	Released under the MIT License.
*/
'use strict';

const loadConfigFile = require('./shared/loadConfigFile.js');
require('path');
require('process');
require('url');
require('./shared/rollup.js');
require('perf_hooks');
require('crypto');
require('fs');
require('events');
require('tty');
require('./shared/mergeOptions.js');



module.exports = loadConfigFile.loadAndParseConfigFile;
//# sourceMappingURL=loadConfigFile.js.map
