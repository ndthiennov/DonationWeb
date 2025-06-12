import { Routes } from '@angular/router';
import { AuthGuard } from './Guards/auth.guard';
import { LoginComponent } from './Auth/login/login.component';
import { UsersComponent } from './Pages/users/users.component';
import { AdminComponent } from './Pages/users/admin/admin.component';
import { DonorComponent } from './Pages/users/donor/donor.component';
import { OrganiserComponent } from './Pages/users/organiser/organiser.component';
import { PagesComponent } from './Pages/pages.component';
import { CampaignComponent } from './Pages/campaign/campaign.component';
import { TransferenceComponent } from './Pages/transference/transference.component';
import { PostComponent } from './Pages/post/post.component';
import { MapComponent } from './Pages/map/map.component';
import { UncensoredComponent } from './Pages/users/uncensored/uncensored.component';
import { RecipientComponent } from './Pages/users/recipient/recipient.component';

export const routes: Routes = [
    {path:'', component:PagesComponent, 
        children:[
            {path:'users',component:UsersComponent,
                children:[
                    {path:'admin',component:AdminComponent},
                    {path:'donor',component:DonorComponent},
                    {path:'organiser',component:OrganiserComponent},
                    {path:'recipient',component:RecipientComponent},
                    {path:'uncensored',component:UncensoredComponent},
                    {path:'', redirectTo:'admin', pathMatch:'full'},
                  ]
            },
            {path:'campaign', component:CampaignComponent},
            // {path:'transference', component:TransferenceComponent},
            {path:'post', component:PostComponent},
            {path:'map', component:MapComponent},
            {path:'', redirectTo:'users', pathMatch:'full'},
          ],
        canActivate:[AuthGuard],data:{permittedRoles:['admin']}
    },
    {path:'login', component:LoginComponent},
];