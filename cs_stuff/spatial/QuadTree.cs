﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class QuadTree<E> where E:class
{

	private QuadNode<E> _root_node;

	public QuadTree(Vector2 ul, Vector2 lr)
	{
		_root_node = new QuadNode<E>(ul, lr);
		_root_node.Parent = null;
	}

	public void build_quad_tree(int tiers)
	{
		_root_node.subdivide();
		grow_tree(_root_node, tiers);
	}

	/// <summary>
	/// grows a tree of n-tiers from current node
	/// </summary>
	/// <param name="node">node to grow from</param>
	/// <param name="tiers">number of tiers to grow</param>
	private void grow_tree(QuadNode<E> node, int tiers)
	{
		if (tiers == 0)
		{
			return;
		}
		else
		{
			node.Q1.subdivide();
			grow_tree(node.Q1, tiers - 1);
			node.Q2.subdivide();
			grow_tree(node.Q2, tiers - 1);
			node.Q3.subdivide();
			grow_tree(node.Q3, tiers - 1);
			node.Q4.subdivide();
			grow_tree(node.Q4, tiers - 1);
		}
	}

	/// <summary>
	/// retrieves the contents at a given leaf node containing the given point
	/// </summary>
	/// <param name="location">point the leaf node contains</param>
	/// <returns>successful or not</returns>
	public List<E> retrieveContentsAtLocation(Vector2 location)
	{
		return retrieveContentsAtLocation(_root_node,location);
	}

	/// <summary>
	/// retrieves the contents at a given leaf node containing the given point
	/// </summary>
	/// <param name="node">node being searched</param>
	/// <param name="location">point the leaf node contains</param>
	/// <returns>list of content</returns>
	private List<E> retrieveContentsAtLocation(QuadNode<E> node, Vector2 location)
	{

		if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
		{
			return node.Contents;
		}
		else
		{
			//List<E> temp = new List<E>();
			if (node.Q1.contains(location))
				return retrieveContentsAtLocation(node.Q1, location);
			if (node.Q2.contains(location))
				return retrieveContentsAtLocation(node.Q2, location);
			if (node.Q3.contains(location))
				return retrieveContentsAtLocation(node.Q3, location);
			if (node.Q4.contains(location))
				return retrieveContentsAtLocation(node.Q4, location);

			return null;
		}
	}

	/// <summary>
	/// retrieves all contents at the location
	/// </summary>
	/// <param name="location">location of content</param>
	/// <returns>list of content</returns>
	public List<E> retrieveAllContents(Vector2 location)
	{
		List<E> contents = new List<E>();
		contents = retrieveAllContents(_root_node, location, contents);
		return contents;
	}

	/// <summary>
	/// retrieves all content decending from the given node
	/// </summary>
	/// <param name="node">node content to be retrieved from</param>
	/// <param name="location">location of the content</param>
	/// <param name="contents">list of all known content descended from the searched node</param>
	/// <returns>a list of content</returns>
	private List<E> retrieveAllContents(QuadNode<E> node, Vector2 location, List<E> contents)
	{
		if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
		{
			return node.Contents;
		}
		else
		{
			contents.AddRange(retrieveAllContents(node.Q1, location, contents));
			contents.AddRange(retrieveAllContents(node.Q2, location, contents));
			contents.AddRange(retrieveAllContents(node.Q3, location, contents));
			contents.AddRange(retrieveAllContents(node.Q4, location, contents));
			return contents;
		}
	}

	/// <summary>
	/// sets a piece of content to the leaf node containing the given point
	/// </summary>
	/// <param name="content">content to place</param>
	/// <param name="location">location contained by the leaf node</param>
	/// <returns>node it was set to if successful, otherwise null</returns>
	public QuadNode<E> setContentAtLocation(E content, Vector2 location)
	{
		return setContentAtLocation(_root_node,content,location);
	}

	/// <summary>
	/// sets the content at the leaf node descendant from the given node
	/// </summary>
	/// <param name="node">given node</param>
	/// <param name="val">content to be set</param>
	/// <param name="point">location of the content</param>
	/// <returns>the leaf node set or null if set failed or content already exists</returns>
	private QuadNode<E> setContentAtLocation(QuadNode<E> node, E val, Vector2 point)
	{
		if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
		{
			if (node.contains(point))
			{
				if (node.Contents.Contains (val) == false) {
					//Debug.Log ("added...");
					node.Contents.Add (val);
				}
				return node;
			}
			else
			{
				return null;
			}

		}
		else
		{
			if (node.Q1.contains(point))
				return setContentAtLocation(node.Q1, val, point);
			if (node.Q2.contains(point))
				return setContentAtLocation(node.Q2, val, point);
			if (node.Q3.contains(point))
				return setContentAtLocation(node.Q3, val, point);
			if (node.Q4.contains(point))
				return setContentAtLocation(node.Q4, val, point);
			return null;
		}
	}

	/// <summary>
	/// locate the leaf node at the given point
	/// </summary>
	/// <param name="point">point to check</param>
	/// <returns>leaf node containing point</returns>
	public QuadNode<E> locateNode(Vector2 point)
	{
		return locateNode(_root_node, point);
	}

	/// <summary>
	/// locates a leaf node descendant of the current node that contains the given point
	/// </summary>
	/// <param name="node">node being searched</param>
	/// <param name="point">point searched for</param>
	/// <returns>leaf node containing the point</returns>
	private QuadNode<E> locateNode(QuadNode<E> node, Vector2 point)
	{
		if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
		{
			return node;
		}
		else
		{
			if (node.Q1.contains(point))
				return locateNode(node.Q1, point);
			if (node.Q2.contains(point))
				return locateNode(node.Q2, point);
			if (node.Q3.contains(point))
				return locateNode(node.Q3, point);
			if (node.Q4.contains(point))
				return locateNode(node.Q4, point);
			return null;
		}
	}


	/// <summary>
	/// remove specific contents of a node
	/// </summary>
	/// <param name="node">node contents to remove</param>
	/// <param name="val">content to be removed</param>
	private void remove(QuadNode<E> node, E val)
	{
		node.Contents.Remove(val);
	}

	/// <summary>
	/// search nodes within a range of a point, return a list of all contents
	/// </summary>
	/// <param name="point">point from which range check is being performed</param>
	/// <param name="range">length of the range check</param>
	/// <returns>List of contents within range</returns>
	public List<E> findAllWithinRange (Vector2 point, float range)
	{
		List<E> contents = new List<E> ();
		try {
			contents = allWithinRange(_root_node, point, range);
		} catch (Exception e) {
			Debug.Log(e.ToString());
		}
		return contents;
	}

	/// <summary>
	/// search nodes within a range of a point, return a list of all contents
	/// </summary>
	/// <param name="node">node being searched</param>
	/// <param name="point">point from which range check is being performed</param>
	/// <param name="range">length of the range check</param>
	/// <returns>List of contents within range</returns>
	private List<E> allWithinRange(QuadNode<E> node, Vector2 point, float range)
	{
		if (node.Q1 == null || node.Q2 == null || node.Q3 == null || node.Q4 == null)
		{
			//return contents
			return node.Contents;
		}
		else
		{
			List<E> contents = new List<E>();

			//determine if nodes are within range
			if(nodeWithinRange(node.Q1,point,range))
				contents.AddRange(allWithinRange(node.Q1,point,range));
			if (nodeWithinRange(node.Q2, point, range))
				contents.AddRange(allWithinRange(node.Q2, point, range));
			if (nodeWithinRange(node.Q3, point, range))
				contents.AddRange(allWithinRange(node.Q3, point, range));
			if (nodeWithinRange(node.Q4, point, range))
				contents.AddRange(allWithinRange(node.Q4, point, range));

			//return any nodes within range
			return contents;
		}
	}

	/// <summary>
	/// runs three fail-over range checks for a given node
	/// </summary>
	/// <param name="node">node to check</param>
	/// <param name="point">location from which range check is made</param>
	/// <param name="range">the length of the range check</param>
	/// <returns>true if in range, false if not</returns>
	private bool nodeWithinRange(QuadNode<E> node, Vector2 point, float range)
	{
		if (node.contains(point))
			return true;
		else if (Vector2.Distance(node.Center, point) <= range)
			return true;
		else
		{
			//Debug.Log ("nodeWithinRange :: checking here...");
			//get the vector from the point to the node's center
			Vector2 pToC = node.Center - point;
			pToC.Normalize();

			//find the closest position to the node center that is at max range from the point
			pToC = point + pToC * range;

			//if that point is within the node, then the node is within range
			if (node.contains(pToC))
				return true;
		}

		//node not within range
		return false;
	}
}

