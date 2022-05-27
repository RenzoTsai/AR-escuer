using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Music;


namespace MenuController {
	public class MenuManager : MonoBehaviour
	{
		private StateMachine stateMachine;
		public GameObject notification;
		public GameObject navigation;
		public GameObject alarm;
		public Timer timer;
		public StateMachine.ProcessState state;
		private StateMachine.ProcessState last_state;
		public static string selectedSong;
		private float startTime;
		public GameObject worldMap;
		public int delayForRecording = 30;

	    // Start is called before the first frame update
	    void Start()
	    {
	    	stateMachine = new StateMachine();
	    	stateMachine.Process();
	    	alarm.SetActive(false);
	    	notification.SetActive(false);
	    	navigation.SetActive(false);
	    	worldMap.SetActive(false);
	    	startTime = Time.time;
	    }



	    // Update is called once per frame
	    void Update()
	    {
	    	// state = stateMachine.MoveNext();
			if (Time.time - startTime > delayForRecording + 10) 
	    	{
	    		navigation.SetActive(true);
	    		worldMap.SetActive(true);
	    		notification.SetActive(false);
	    	}
	    	else if (Time.time - startTime > delayForRecording + 5) 
	    	{
	    		notification.SetActive(true);
	    		alarm.SetActive(false);
	    	}
	    	else if (Time.time - startTime > delayForRecording) 
	    	{
	    		alarm.SetActive(true);
	    	}
	    	

	    	// Debug.Log(state);

	    	// if (Input.anyKey) {
	    	// 	if (state == StateMachine.ProcessState.Init) 
		    // 	{
			   //  	notification.SetActive(false);
			   //  	navigation.SetActive(false);
		    // 	}
		    // 	else if (state == StateMachine.ProcessState.Room) 
		    // 	{
			   //  	notification.SetActive(true);
			   //  	navigation.SetActive(false);
		    // 	}
		    // 	else if (state == StateMachine.ProcessState.LivingRoom)
		    // 	{
			   //  	notification.SetActive(false);
			   //  	navigation.SetActive(true);
		    // 	}
		    // }

	    }

	    public static string GetSelectedSong()
	    {
	    	if (selectedSong == "" || selectedSong == null)
	    	{
	    		return null;
	    		
	    	}
	    	return selectedSong;

	    }
	}


	public enum Command
	{
		Left,
		Right,
		Up,
		Down,
		Invalid,
	}

	public class StateMachine
	{
		public enum ProcessState
		{
		    Init,
		    Room,
		    LivingRoom,
		    Kitchen,
		    Light,
		    Fan,
		    Music,
		    MusicList,
		    Tea,
		    Invalid
		}

		public static Command currentInput;


		class StateTransition
		{
		    readonly ProcessState CurrentState;
		    readonly Command Command;

		    public StateTransition(ProcessState currentState, Command command)
		    {
		        CurrentState = currentState;
		        Command = command;
		    }

		    public override int GetHashCode()
		    {
		        return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
		    }

		    public override bool Equals(object obj)
		    {
		        StateTransition other = obj as StateTransition;
		        return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
		    }
		}

		Dictionary<StateTransition, ProcessState> transitions;
		public ProcessState CurrentState { get; private set; }

		public void Process()
		{
		    CurrentState = ProcessState.Init;
		    transitions = new Dictionary<StateTransition, ProcessState>
		    {
		        { new StateTransition(ProcessState.Init, Command.Left), ProcessState.Room},
		        { new StateTransition(ProcessState.Init, Command.Right), ProcessState.Room},
		        { new StateTransition(ProcessState.Init, Command.Up), ProcessState.Room},
		        { new StateTransition(ProcessState.Init, Command.Down), ProcessState.Room},
		        { new StateTransition(ProcessState.Room, Command.Left), ProcessState.Init},
		        { new StateTransition(ProcessState.Room, Command.Right), ProcessState.LivingRoom},
		        { new StateTransition(ProcessState.Room, Command.Down), ProcessState.Kitchen},
		        { new StateTransition(ProcessState.LivingRoom, Command.Left), ProcessState.Room},
		        { new StateTransition(ProcessState.LivingRoom, Command.Right), ProcessState.Fan},
		        { new StateTransition(ProcessState.LivingRoom, Command.Up), ProcessState.Light},
		        { new StateTransition(ProcessState.LivingRoom, Command.Down), ProcessState.Music},
		        { new StateTransition(ProcessState.Light, Command.Left), ProcessState.LivingRoom},
		        { new StateTransition(ProcessState.Fan, Command.Left), ProcessState.LivingRoom},
		        { new StateTransition(ProcessState.Music, Command.Left), ProcessState.LivingRoom},
		        { new StateTransition(ProcessState.Music, Command.Right), ProcessState.MusicList},
		        { new StateTransition(ProcessState.MusicList, Command.Right), ProcessState.Music},
		        { new StateTransition(ProcessState.MusicList, Command.Left), ProcessState.Music},
		        { new StateTransition(ProcessState.Kitchen, Command.Right), ProcessState.Tea},
		        { new StateTransition(ProcessState.Tea, Command.Left), ProcessState.Kitchen},
		        { new StateTransition(ProcessState.Kitchen, Command.Left), ProcessState.Room},
		    };
		}

		public static Command ParseCommand(){

			if (Input.GetKeyDown(KeyCode.DownArrow))
	    	{
	    		return Command.Down;
	    	} 
	    	else if (Input.GetKeyDown(KeyCode.UpArrow)) 
	    	{
	    		return Command.Up;
	    	} 
	    	else if (Input.GetMouseButtonDown(0)) 
	    	{
	    		return Command.Left;
	    	}
	    	else if (Input.GetMouseButtonDown(1)) 
	    	{
	    		return Command.Right;
	    	}
	    	else 
	    	{
	    		return Command.Invalid;
	    	}

		}

		public ProcessState GetNext(Command command)
		{
		    StateTransition transition = new StateTransition(CurrentState, command);
		    ProcessState nextState;
			if (transitions.TryGetValue(transition, out nextState)) 
			{
				return nextState;
			}
			else
			{
				return ProcessState.Invalid;
			}
		    
		}

		public ProcessState MoveNext()
		{
			Command command = ParseCommand();
			ProcessState tempState;
			if (command != Command.Invalid) {
				tempState = GetNext(command);
				if (tempState != ProcessState.Invalid) {
					CurrentState = tempState;
				}
			}
			return CurrentState;

		}
	}
}