/**
 * @fileoverview This file is generated by the Angular 2 template compiler.
 * Do not edit.
 * @suppress {suspiciousCode,uselessCode,missingProperties}
 */
 /* tslint:disable */

import * as import0 from '@angular/core/src/linker/ng_module_factory';
import * as import1 from '../../app/app-routing.module';
import * as import2 from '@angular/router/src/router_module';
import * as import3 from '@angular/common/src/location/location';
import * as import4 from '@angular/router/src/url_tree';
import * as import5 from '@angular/router/src/router_outlet_map';
import * as import6 from '@angular/core/src/linker/system_js_ng_module_factory_loader';
import * as import7 from '@angular/router/src/router_preloader';
import * as import8 from '@angular/core/src/di/injector';
import * as import9 from './app.component.ngfactory';
import * as import10 from './components/home.component.ngfactory';
import * as import11 from './components/take-quiz.component.ngfactory';
import * as import12 from './components/dashboard.component.ngfactory';
import * as import13 from './components/quizes.component.ngfactory';
import * as import14 from '@angular/common/src/location/platform_location';
import * as import15 from '@angular/common/src/location/location_strategy';
import * as import16 from '@angular/core/src/linker/compiler';
import * as import17 from '../../app/app.component';
import * as import18 from '../../app/components/home.component';
import * as import19 from '../../app/components/take-quiz.component';
import * as import20 from '../../app/components/dashboard.component';
import * as import21 from '../../app/app.authguard';
import * as import22 from '../../app/components/quizes.component';
import * as import23 from '@angular/core/src/application_ref';
import * as import24 from '@angular/router/src/url_handling_strategy';
import * as import25 from '@angular/router/src/route_reuse_strategy';
import * as import26 from '@angular/router/src/router';
import * as import27 from '@angular/core/src/linker/ng_module_factory_loader';
import * as import28 from '@angular/router/src/router_config_loader';
import * as import29 from '@angular/router/src/router_state';
import * as import30 from '@angular/core/src/application_tokens';
class AppRoutingModuleInjector extends import0.NgModuleInjector<import1.AppRoutingModule> {
  _ROUTER_FORROOT_GUARD_0:any;
  _RouterModule_1:import2.RouterModule;
  _AppRoutingModule_2:import1.AppRoutingModule;
  __ROUTER_CONFIGURATION_3:any;
  __LocationStrategy_4:any;
  __Location_5:import3.Location;
  __UrlSerializer_6:import4.DefaultUrlSerializer;
  __RouterOutletMap_7:import5.RouterOutletMap;
  __NgModuleFactoryLoader_8:import6.SystemJsNgModuleLoader;
  __ROUTES_9:any[];
  __Router_10:any;
  __ActivatedRoute_11:any;
  _NoPreloading_12:import7.NoPreloading;
  _PreloadingStrategy_13:any;
  _RouterPreloader_14:import7.RouterPreloader;
  __PreloadAllModules_15:import7.PreloadAllModules;
  __NgProbeToken_16:any[];
  __ROUTER_INITIALIZER_17:any;
  __APP_BOOTSTRAP_LISTENER_18:any[];
  constructor(parent:import8.Injector) {
    super(parent,[
      import9.AppComponentNgFactory,
      import10.HomeComponentNgFactory,
      import11.TakeQuizComponentNgFactory,
      import12.DashboardComponentNgFactory,
      import13.QuizesComponentNgFactory
    ]
    ,([] as any[]));
  }
  get _ROUTER_CONFIGURATION_3():any {
    if ((this.__ROUTER_CONFIGURATION_3 == null)) { (this.__ROUTER_CONFIGURATION_3 = {}); }
    return this.__ROUTER_CONFIGURATION_3;
  }
  get _LocationStrategy_4():any {
    if ((this.__LocationStrategy_4 == null)) { (this.__LocationStrategy_4 = import2.provideLocationStrategy(this.parent.get(import14.PlatformLocation),this.parent.get(import15.APP_BASE_HREF,(null as any)),this._ROUTER_CONFIGURATION_3)); }
    return this.__LocationStrategy_4;
  }
  get _Location_5():import3.Location {
    if ((this.__Location_5 == null)) { (this.__Location_5 = new import3.Location(this._LocationStrategy_4)); }
    return this.__Location_5;
  }
  get _UrlSerializer_6():import4.DefaultUrlSerializer {
    if ((this.__UrlSerializer_6 == null)) { (this.__UrlSerializer_6 = new import4.DefaultUrlSerializer()); }
    return this.__UrlSerializer_6;
  }
  get _RouterOutletMap_7():import5.RouterOutletMap {
    if ((this.__RouterOutletMap_7 == null)) { (this.__RouterOutletMap_7 = new import5.RouterOutletMap()); }
    return this.__RouterOutletMap_7;
  }
  get _NgModuleFactoryLoader_8():import6.SystemJsNgModuleLoader {
    if ((this.__NgModuleFactoryLoader_8 == null)) { (this.__NgModuleFactoryLoader_8 = new import6.SystemJsNgModuleLoader(this.parent.get(import16.Compiler),this.parent.get(import6.SystemJsNgModuleLoaderConfig,(null as any)))); }
    return this.__NgModuleFactoryLoader_8;
  }
  get _ROUTES_9():any[] {
      if ((this.__ROUTES_9 == null)) { (this.__ROUTES_9 = [[
        {
          path: 'app',
          component: import17.AppComponent
        }
        ,
        {
          path: 'home',
          component: import18.HomeComponent
        }
        ,
        {
          path: 'take-quiz/:id',
          component: import19.TakeQuizComponent
        }
        ,
        {
          path: 'dashboard',
          component: import20.DashboardComponent,
          canActivate: [import21.AuthGuard]
        }
        ,
        {
          path: 'quizes',
          component: import22.QuizesComponent,
          canActivate: [import21.AuthGuard]
        }
        ,
        {
          path: '',
          redirectTo: '/home',
          pathMatch: 'full'
        }
        ,
        {
          path: '**',
          redirectTo: '/home'
        }

      ]
    ]); }
    return this.__ROUTES_9;
  }
  get _Router_10():any {
    if ((this.__Router_10 == null)) { (this.__Router_10 = import2.setupRouter(this.parent.get(import23.ApplicationRef),this._UrlSerializer_6,this._RouterOutletMap_7,this._Location_5,this,this._NgModuleFactoryLoader_8,this.parent.get(import16.Compiler),this._ROUTES_9,this._ROUTER_CONFIGURATION_3,this.parent.get(import24.UrlHandlingStrategy,(null as any)),this.parent.get(import25.RouteReuseStrategy,(null as any)))); }
    return this.__Router_10;
  }
  get _ActivatedRoute_11():any {
    if ((this.__ActivatedRoute_11 == null)) { (this.__ActivatedRoute_11 = import2.rootRoute(this._Router_10)); }
    return this.__ActivatedRoute_11;
  }
  get _PreloadAllModules_15():import7.PreloadAllModules {
    if ((this.__PreloadAllModules_15 == null)) { (this.__PreloadAllModules_15 = new import7.PreloadAllModules()); }
    return this.__PreloadAllModules_15;
  }
  get _NgProbeToken_16():any[] {
    if ((this.__NgProbeToken_16 == null)) { (this.__NgProbeToken_16 = [import2.routerNgProbeToken()]); }
    return this.__NgProbeToken_16;
  }
  get _ROUTER_INITIALIZER_17():any {
    if ((this.__ROUTER_INITIALIZER_17 == null)) { (this.__ROUTER_INITIALIZER_17 = import2.initialRouterNavigation(this._Router_10,this.parent.get(import23.ApplicationRef),this._RouterPreloader_14,this._ROUTER_CONFIGURATION_3)); }
    return this.__ROUTER_INITIALIZER_17;
  }
  get _APP_BOOTSTRAP_LISTENER_18():any[] {
    if ((this.__APP_BOOTSTRAP_LISTENER_18 == null)) { (this.__APP_BOOTSTRAP_LISTENER_18 = [this._ROUTER_INITIALIZER_17]); }
    return this.__APP_BOOTSTRAP_LISTENER_18;
  }
  createInternal():import1.AppRoutingModule {
    this._ROUTER_FORROOT_GUARD_0 = import2.provideForRootGuard(this.parent.get(import26.Router,(null as any)));
    this._RouterModule_1 = new import2.RouterModule(this._ROUTER_FORROOT_GUARD_0);
    this._AppRoutingModule_2 = new import1.AppRoutingModule();
    this._NoPreloading_12 = new import7.NoPreloading();
    this._PreloadingStrategy_13 = this._NoPreloading_12;
    this._RouterPreloader_14 = new import7.RouterPreloader(this._Router_10,this._NgModuleFactoryLoader_8,this.parent.get(import16.Compiler),this,this._PreloadingStrategy_13);
    return this._AppRoutingModule_2;
  }
  getInternal(token:any,notFoundResult:any):any {
    if ((token === import2.ROUTER_FORROOT_GUARD)) { return this._ROUTER_FORROOT_GUARD_0; }
    if ((token === import2.RouterModule)) { return this._RouterModule_1; }
    if ((token === import1.AppRoutingModule)) { return this._AppRoutingModule_2; }
    if ((token === import2.ROUTER_CONFIGURATION)) { return this._ROUTER_CONFIGURATION_3; }
    if ((token === import15.LocationStrategy)) { return this._LocationStrategy_4; }
    if ((token === import3.Location)) { return this._Location_5; }
    if ((token === import4.UrlSerializer)) { return this._UrlSerializer_6; }
    if ((token === import5.RouterOutletMap)) { return this._RouterOutletMap_7; }
    if ((token === import27.NgModuleFactoryLoader)) { return this._NgModuleFactoryLoader_8; }
    if ((token === import28.ROUTES)) { return this._ROUTES_9; }
    if ((token === import26.Router)) { return this._Router_10; }
    if ((token === import29.ActivatedRoute)) { return this._ActivatedRoute_11; }
    if ((token === import7.NoPreloading)) { return this._NoPreloading_12; }
    if ((token === import7.PreloadingStrategy)) { return this._PreloadingStrategy_13; }
    if ((token === import7.RouterPreloader)) { return this._RouterPreloader_14; }
    if ((token === import7.PreloadAllModules)) { return this._PreloadAllModules_15; }
    if ((token === import23.NgProbeToken)) { return this._NgProbeToken_16; }
    if ((token === import2.ROUTER_INITIALIZER)) { return this._ROUTER_INITIALIZER_17; }
    if ((token === import30.APP_BOOTSTRAP_LISTENER)) { return this._APP_BOOTSTRAP_LISTENER_18; }
    return notFoundResult;
  }
  destroyInternal():void {
    this._RouterPreloader_14.ngOnDestroy();
  }
}
export const AppRoutingModuleNgFactory:import0.NgModuleFactory<import1.AppRoutingModule> = new import0.NgModuleFactory(AppRoutingModuleInjector,import1.AppRoutingModule);