var gulp = require('gulp'),
	gutil = require('gulp-util');

var paths = {
	bower: {
		root: './bower_components/',
		components: {
			'bootstrap': 'bootstrap/dist/**/*.{js,map,css,tts,svg,woff,eot}',
			'jquery': 'jquery/dist/jquery*.{js,map}',
			'angular': 'angular/angular*.{js,map}',
			'angular-route': 'angular-route/angular-route*.{js,map}' 
		}
	},
	lib: './src/lib/'	
};

gulp.task('copy-bower', function() {
	
	var log = function(src, dest) { return function() { gutil.log("copied '" + src + "' to '" + dest + "'"); }; };
	
	for(var componentName in paths.bower.components) {
		
		var componentSrc = paths.bower.root + paths.bower.components[componentName];
		var componentDest = paths.lib + componentName;
		
		gulp.src(componentSrc)
			.pipe(gulp.dest(componentDest))
			.on('end', log(componentSrc, componentDest));		
		
	}
	
});

gulp.task('default', ['copy-bower']);
