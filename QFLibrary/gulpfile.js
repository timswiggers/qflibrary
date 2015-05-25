/// <binding AfterBuild='compile-less' ProjectOpened='copy-bower' />
var gulp = require('gulp'),
	gutil = require('gulp-util'),
	less = require('gulp-less');

var paths = {
	bower: {
		root: './bower_components/',
		components: {
			'bootstrap': 'bootstrap/dist/**/*.{js,map,css,tts,svg,woff,eot}',
			'jquery': 'jquery/dist/jquery*.{js,map}',
			'angular': 'angular/angular*.{js,map}',
			'angular-route': 'angular-route/angular-route*.{js,map}' ,
			'angular-cookies': 'angular-cookies/angular-cookies*.{js,map}'
		}
	},
	less: './src/less/**/*.less',
	out: {
		lib: './src/lib/',
		css: './src/css/'	
	}	
};

gulp.task('copy-bower', function() {
	
	var log = function(src, dest) { return function() { gutil.log("copied '" + src + "' to '" + dest + "'"); }; };
	
	for(var componentName in paths.bower.components) {
		
		var componentSrc = paths.bower.root + paths.bower.components[componentName];
		var componentDest = paths.out.lib + componentName;
		
		gulp.src(componentSrc)
			.pipe(gulp.dest(componentDest))
			.on('end', log(componentSrc, componentDest));		
		
	}
	
});

gulp.task('compile-less', function(){
	gulp.src(paths.less)
		.pipe(less())
		.pipe(gulp.dest(paths.out.css))
})

gulp.task('default', ['copy-bower', 'compile-less']);
