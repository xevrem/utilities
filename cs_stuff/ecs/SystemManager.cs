﻿//
//  SystemManager.cs
//
//  Author:
//       erika <>
//
//  Copyright (c) 2016 erika
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;


public class SystemManager
{
	private List<EntitySystem> _systems;
	private ECSInstance _ecs_instance;

	public SystemManager (ECSInstance instance)
	{
		this._ecs_instance = instance;
		this._systems = new List<EntitySystem> ();
	}

	public EntitySystem set_system(EntitySystem system, params IComponent[] components){
		//TODO add the system and assign its components.
		foreach (IComponent c in components) {
			this._ecs_instance.component_manager.register_component_type (c);
			system.component_types.Add (c.type_id);
		}
		system.ecs_instance = this._ecs_instance;
		this._systems.Add (system);
		return system;
	}


	public void initialize_systems(){
		foreach (EntitySystem system in this._systems) {
			system.initialize_system ();
		}
	}

	public void systems_load_content(){
		foreach (EntitySystem system in this._systems) {
			system.load_content ();
		}
	}

	public void resolve(Entity e){
		bool valid;

		foreach (EntitySystem system in this._systems) {
			valid = true;

			foreach (int type_id in system.component_types) {
				valid &= this._ecs_instance.has_component (e, type_id);
			}

			if (valid) {
				
				system.add_entity(e);

			}
		}
	}

	public void delete_entity(Entity e){
		//FIXME: very innefficient... find a better method
		foreach (EntitySystem system in this._systems) {
			system.remove_entity (e);
		}
	}

	public void clean_up(){
		foreach (EntitySystem system in this._systems) {
			system.clean_system ();
		}
		this._systems.Clear ();
	}
}


